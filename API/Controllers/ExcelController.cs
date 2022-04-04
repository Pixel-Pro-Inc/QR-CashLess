using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using API.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace API.Controllers
{
    public class ExcelController: BaseApiController
    {
        private readonly string _rootPath;
        public ExcelController(IWebHostEnvironment env) 
        {
            _rootPath = env.WebRootPath;
        }

        [HttpGet("export/{branchId}")]//This is used as reference for http calls that is api/excel/export will call the ExportData() method
        public async Task<IActionResult> ExportData(string branchId)
        {
            //Remove previously stored files if any
            if (Directory.Exists(savePath("Rodizio Express Data_Export")))
            {
                Directory.Delete(savePath("Rodizio Express Data_Export"), true);
            }

            Excel ex = new Excel();//Creates new instance of Excel class

            ex.CreateNewFile();//Creates a new excel file

            string[] worksheetNames = { "CompletedOrders", "CancelledOrders", "UnCompletedOrders" };

            int emptyCount = 0;
            for (int i = 0; i < worksheetNames.Length; i++)
            {
                //Gets Orders from the Database
                string dir = worksheetNames[i] == "UnCompletedOrders" ? "Order" : worksheetNames[i];
                List<List<OrderItem>> orderItems = await GetOrders(dir + "/", branchId);

                CreateWorkSheet(ex, worksheetNames[i], orderItems);
            }            
            
            string folderName = "Rodizio Express Data_Export";//Generates folder name

            string fileName = "Rodizio Express Data_Export/Rodizio Express Data_Export " + DateTime.Now.ToShortDateString().Replace('/', '-') + ".xlsx";//Generates file name

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

            return File(memory, GetContentType(filePath), "Rodizio Express Data_Export " + DateTime.Now.ToShortDateString().Replace('/', '-') + ".xlsx");
        }

        private void CreateWorkSheet(Excel ex, string worksheetName, List<List<OrderItem>> orderItems)
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

            //Writes the fields to the excel sheet
            for (int i = 0; i < fields.Count; i++)
            {
                string field = fields[i];

                if(fields[i] == "Weight")
                {
                    field = "Weight (grams)";
                }
                else if(fields[i] == "Price")
                {
                    field = "Price (BWP)";
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

                        if (data.Key == "OrderDateTime")
                        {
                            DateTime oDate = (DateTime)data.Value;
                            oDate = oDate.AddHours(2);

                            ex.WriteToCell(rowCount, x, data.Value == null ? "-" : oDate.ToString(), worksheetName);// writes data to excel cell using row and column as reference (row, column, data)
                            continue;
                        }

                        ex.WriteToCell(rowCount, x, data.Value == null ? "-" : data.Value.ToString(), worksheetName);// writes data to excel cell using row and column as reference (row, column, data)
                    }

                    rowCount++;
                }               
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

        string savePath(string fileName) { return Path.Combine(_rootPath, fileName); }

        private async Task<List<List<OrderItem>>> GetOrders(string path, string branchId) 
        {
            return await _firebaseDataContext.GetData<List<OrderItem>>(path + branchId);
        }
    }
}
