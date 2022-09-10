using API.Controllers;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using RodizioSmartKernel.Entities;
using RodizioSmartKernel.Entities.Aggregates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class ExcelService: BaseService, IExcelService
    {

        private readonly string _rootPath;
        private readonly IFirebaseServices _firebaseServices;

        public ExcelService(IFirebaseServices firebaseServices, IWebHostEnvironment env)
        {
            _firebaseServices = firebaseServices;
            _rootPath = env.WebRootPath;
        }

        public async Task<FileStreamResult> ExportDataFromDatabase(string branchId)
        {
            Excel ex= await MakeExcelFile(branchId);
            return await SetExcelFile(ex);
        }
        public async Task<FileStreamResult> ExportDataFromDatabase(List<Order> Orders)
        {
            Excel ex =  MakeExcelFile(Orders);
            return await SetExcelFile(ex);
        }


        /// <summary>
        /// This takses in the <paramref name="branchId"/>, and creates an excel based on all the orders in the specific branch
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        private async Task<Excel> MakeExcelFile(string branchId)
        {
            Excel ex = new Excel();
            ex.CreateNewFile();//Creates a new excel file

            string[] worksheetNames = { "CompletedOrders", "CancelledOrders", "UnCompletedOrders" };

            // Creates a worksheet for every type of orders described in the worksheet names
            int emptyCount = 0;
            for (int i = 0; i < worksheetNames.Length; i++)
            {
                //Gets Orders from the Database
                string worksheetName = worksheetNames[i] == "UnCompletedOrders" ? "Order" : worksheetNames[i];

                List<Order> Orders = await _firebaseServices.GetDataArray<Order,OrderItem>(worksheetName + "/" + branchId);
                if (Orders.Count <= 0) //Checks to see if the result from the database actually has data
                {
                    emptyCount++;
                    continue;
                }
                CreateWorkSheet(ex, worksheetNames[i], Orders);
                if (emptyCount == worksheetNames.Length)
                    throw new NullReferenceException("There was no data on the order paths/ worksheetnames provided" +
                        $"The follow are some of them: {worksheetNames[i]}, {worksheetNames[i + 2]}");

            }
            return ex;
        }
        /// <summary>
        /// This is an overload of <see cref="MakeExcelFile(string)"/> that takes in a specific list of orders and makes an excel file out of them 
        /// </summary>
        /// <param name="Ordres"></param>
        /// <returns></returns>
        private Excel MakeExcelFile(List<Order> Orders)
        {
            Excel ex = new Excel();
            ex.CreateNewFile();//Creates a new excel file
            CreateWorkSheet(ex, "CompletedOrders", Orders);
            return ex;

        }
        private async Task<FileStreamResult> SetExcelFile(Excel ex)
        {

            string folderName = "Rodizio Express Data_Export";//Generates folder name
            string fileName = "Rodizio Express Data_Export/Rodizio Express Data_Export " + DateTime.Now.ToShortDateString().Replace('/', '-') + ".xlsx";//Generates file name

            //Remove previously stored files if any
            if (Directory.Exists(savePath(folderName))) Directory.Delete(savePath(folderName), true);

            Directory.CreateDirectory(savePath(folderName));//Creates directory

            System.IO.File.SetAttributes(savePath(folderName), FileAttributes.Normal);//Removes special priveledges for folder

            ex.SaveAs(savePath(fileName));//Saves file to local storage

            System.IO.File.SetAttributes(savePath(fileName), FileAttributes.Normal);//Removes special priveledges for file

            var filePath = savePath(fileName);

            var memory = new MemoryStream();//Creates new Memory stream

            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);//copies the file at that location to memory stream
            }

            memory.Position = 0;

            return new BaseApiController( _firebaseServices).File(memory, GetContentType(filePath), folderName + DateTime.Now.ToShortDateString().Replace('/', '-') + ".xlsx");
        }
        private void CreateWorkSheet(Excel ex, string worksheetName, List<Order> orderItems)
        {
            ex.CreateWorkSheet(worksheetName);//Creates a new work sheet

            //I used an extension method I got from stackoverflow to convert a class into keyValuePairs
            IDictionary<string, object> keyValuePairs = new OrderItem().AsDictionary();

            //This is a list of all the fields in the excel file displayed on the top row
            List<string> fields = new List<string>();

            //Loop through the dictionary to use all the keys as fields (this is better than using a hard coded list incase changes are made to the object) 
            foreach (KeyValuePair<string, object> entry in keyValuePairs)
            {
                fields.Add(entry.Key);
            }

            //Switches the fields to a format thats usable in the Excel sheet   
            for (int i = 0; i < fields.Count; i++)
            {
                string field = fields[i];

                if (fields[i] == "Weight")
                {
                    field = "Weight (grams)";
                }
                else if (fields[i] == "Price")
                {
                    field = "Price (BWP)";
                }
                else if (fields[i] == "User")
                {
                    field = "Cashier";
                }

                ex.WriteToCell(0, i, field, worksheetName); // writes data to excel cell using row and column as reference (row, column, data)
            }

            int rowCount = 1;//used to keep track of rows

            for (int i = 0; i < orderItems.Count; i++)//Loop through all orders
            {
                var order = orderItems[i]; //Stores individual order temporarily

                foreach (var orderItem in order)//For each order item 
                {
                    for (int x = 0; x < fields.Count; x++)//Loops through all fields
                    {
                        keyValuePairs = orderItem.AsDictionary();//We store it as a dictionary

                        var data = keyValuePairs.Where(d => d.Key == fields[x]).ToArray()[0];//Returns every key value pair where the key is equal to the current field since its a single orderItem only 1 so we get the first index and use that 

                        if (data.Key == "Weight")
                        {
                            ex.WriteToCell(rowCount, x, data.Value == null ? "-" : data.Value.ToString().Replace("grams", ""), worksheetName);// writes data to excel cell using row and column as reference (row, column, data)
                            continue;
                        }

                        if (data.Key == "Chefs" || data.Key == "Sauces")
                        {
                            ex.WriteToCell(rowCount, x, data.Value == null ? "-" : Format.ListToString((List<string>)data.Value), worksheetName);// writes data to excel cell using row and column as reference (row, column, data)
                            continue;
                        }
                        if (fields[x].ToLower() == "ordernumber")
                            ex.WriteToCell(rowCount, x, order[0].OrderNumber, worksheetName);// writes data to excel cell using row and column as reference (row, column, data)

                        if (fields[x].ToLower() == "price (bwp)")
                        {
                            float totalCost = 0;
                            totalCost += float.Parse(orderItem.Price);

                            ex.WriteToCell(rowCount, x, totalCost.ToString(), worksheetName);
                        }

                        ex.WriteToCell(rowCount, x, data.Value == null ? "-" : data.Value.ToString(), worksheetName);// writes data to excel cell using row and column as reference (row, column, data)
                    }

                    rowCount++;
                }
                ex.WriteToCell(rowCount, 0, "Total", worksheetName);
                ex.WriteToCell(rowCount, 1, $"=SUM(B2:B{(orderItems.Count + 1)})", worksheetName);
            }
        }

        private string GetContentType(string path)//Gets the type of the file at that directory
        {
            var provider = new FileExtensionContentTypeProvider();

            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
        private string savePath(string fileName) => Path.Combine(_rootPath, fileName);

    }
}
