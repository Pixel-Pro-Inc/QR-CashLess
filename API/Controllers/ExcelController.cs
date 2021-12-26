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

namespace API.Controllers
{
    public class ExcelController:BaseApiController
    {
        private readonly string dir = "order/";
        public ExcelController():base() { }
        ConnectionChecker connectionChecker = new ConnectionChecker();

        [HttpPost("importexceldata")]
        public void ImportExcelData(string filePath) //takes data in local and then updates firebase, hence why you 'post' to firebase
        {
            if (connectionChecker.CheckConnection())
            {
                Excel excel = new Excel(filePath, 1);
                if ((excel.ReadCell(1, 0)).Trim() == null || (excel.ReadCell(1, 0)).Trim() == "")
                {
                    Console.WriteLine("Use a valid excel file.");
                }
                else
                {
                    int kh = 0;
                    int ghd = 1;
                    while (kh == 0)
                    {
                        if ((excel.ReadCell(ghd, 0)).Trim() == "")
                        {
                            kh = 1;
                        }
                        if (kh == 0)
                        {
                            ghd++;
                        }
                    }
                    List<OrderItem> importData = new List<OrderItem>();
                    foreach (var newline in importData)
                    {
                        for (int i = 1; i < ghd; i++)
                        {
                            newline.OrderNumber = ((excel.ReadCell(i, 0)).Trim()).Trim();
                            //orders don't have branchid
                            newline.PaymentMethod = ((excel.ReadCell(i, 2)).Trim()).Trim();
                            newline.Name = ((excel.ReadCell(i, 3)).Trim()).Trim();
                            newline.Quantity = Int32.Parse(((excel.ReadCell(i, 4)).Trim()).Trim());
                            newline.Price = ((excel.ReadCell(i, 5)).Trim()).Trim();
                            newline.Weight = ((excel.ReadCell(i, 6)).Trim()).Trim();
                            //orders don't have total
                            }
                        }
                    _firebaseDataContext.StoreData(dir, importData); //its not records, find the correct info
                }
                excel.Close();
            }
            else
            {
                Console.WriteLine("There is no internet.");
                return;
            }
        }

        [HttpGet("exportexcelData")]// because you are getting info from the controller, which is getting its info from firebase
        public async Task<ActionResult<Excel>> ExportExcelData(string path)
        {
            Excel ex = new Excel();
            if (connectionChecker.CheckConnection())
            {
                #region makes and exports a excel file
                
                ex.CreateNewFile();
                string[] fields = new string[8];
                fields[0] = "invoice";
                fields[1] = "branchId";
                fields[2] = "paymentMethod";
                fields[3] = "ItemName";
                fields[4] = "Quantity";
                fields[5] = "Price";
                fields[6] = "weight";
                fields[7] = "Total";

                var getTaskResultrd = await _firebaseDataContext.GetData("order"); //its not records, find the correct info

                List<OrderItem> temp = new List<OrderItem>();

                foreach (var item in getTaskResultrd)
                {
                    temp = JsonConvert.DeserializeObject<List<OrderItem>>(((JArray)item).ToString());
                }
                int memCount = 0;
                foreach (var item in temp)
                {
                    if (item.OrderNumber != null && item.Id != 0) memCount++;
                }          
                if (memCount != 0)
                {
                    for (int i = 0; i < fields.Length; i++)
                    {
                        ex.WriteToCell(0, i, fields[i]); // This is to fill in the column tiles
                    }

                    int di = 1;
                    foreach (var cell in temp)
                    {
                        ex.WriteToCell(di, 0, cell.OrderNumber.ToString());
                        ex.WriteToCell(di, 1, cell.OrderNumber.ToString());
                        ex.WriteToCell(di, 2, cell.PaymentMethod.ToString());
                        ex.WriteToCell(di, 3, cell.Name.ToString());
                        ex.WriteToCell(di, 4, cell.Quantity.ToString());
                        ex.WriteToCell(di, 5, cell.Price.ToString());
                        ex.WriteToCell(di, 6, cell.Weight.ToString());
                        ex.WriteToCell(di, 7, cell.OrderNumber.ToString());
                        di++;
                    }
                    ex.SaveAs(path);
                    //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    Console.WriteLine("export completed");
                }
                else
                {
                    Console.WriteLine("There is no data to export.");
                }
                ex.Close();
                return ex;
                #endregion
            }
            else
            {
                Console.WriteLine("There is no internet.");
                return ex;
            }
        }

    }
}
