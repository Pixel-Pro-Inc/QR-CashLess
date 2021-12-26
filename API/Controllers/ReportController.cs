using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ReportController : BaseApiController
    {
        private readonly string dir = "reports/";

        public ReportController():base(){ }
        
        [HttpGet("sales/total")] //this is what is actuallly sent up when dates are given, The Total sales report. It gives all the order numbers and the amount 
        public async Task<ActionResult<SalesDto>> GetTotalSales(ReportDto reportDto)
        {
            //returns order number and amount earned
            List<List<OrderItem>> eligibleOrders = new List<List<OrderItem>>();

            eligibleOrders = await GetOrdersByDate(reportDto);

            List<float> totals = new List<float>();
            List<string> orderNumbers = new List<string>();

            for (int i = 0; i < eligibleOrders.Count; i++)
            {
                string orderNumber = eligibleOrders[i][0].OrderNumber.Substring(eligibleOrders[i][0].OrderNumber.IndexOf('_') + 1, 4);
                orderNumbers.Add(orderNumber);
            }

            for (int i = 0; i < eligibleOrders.Count; i++)
            {
                totals.Add(0);

                foreach (var item in eligibleOrders[i])
                {
                    totals[i] += float.Parse(item.Price);
                }
            }

            SalesDto totalSalesDto = new SalesDto();
            totalSalesDto.orderNumbers = orderNumbers;
            totalSalesDto.orderRevenue = totals;

            return totalSalesDto;
        }

        [HttpGet("sales/date")] //This returns all the orders in that period of time,  Its not really called by the view/client
        public async Task<List<List<OrderItem>>> GetOrdersByDate(ReportDto reportDto)
        {
            List<List<OrderItem>> eligibleOrders = new List<List<OrderItem>>();
            var result = await _firebaseDataContext.GetData(dir + reportDto.branchId);

            List<List<OrderItem>> orders = new List<List<OrderItem>>();

            foreach (var item in result)
            {
                var pain = (List<OrderItem>)item;
                orders.Add(pain);
            }

            foreach (var order in orders)
            {
                for (int i = 0; i < order.Count; i++)
                {
                    var orderItem = order[i];

                    string orderDate = orderItem.OrderNumber.Substring(0, 10);
                    string real = orderDate.Replace("-", "/");
                    int day = Int32.Parse(real.Substring(0, 2));
                    int month = Int32.Parse(real.Substring(3, 2));
                    int year = Int32.Parse(real.Substring(6, 4));
                    DateTime orderTime = new DateTime(year, month, day);

                    if (orderTime >= reportDto.startDate && orderTime <= reportDto.endDate)
                    {
                        eligibleOrders.Add(order);
                        i = order.Count;
                    }
                }
            }

            return eligibleOrders;
        }


        [HttpGet("sales/item")] //This is the sales by item on a time period. It gives item| quantity| amount paid
        public async Task<ActionResult<SalesDto>> GetSalesByItem(ReportDto reportDto)
        {
            List<List<OrderItem>> ordersgiven = new List<List<OrderItem>>();
            ordersgiven = await GetOrdersByDate(reportDto);

            //Dictionary<string, (string, string, int)> orderlist = new Dictionary<string, (string,string, int)>();
            SalesDto SalesByItem = new SalesDto();
            foreach (var order in ordersgiven)
            {
                foreach (var item in order)
                {
                    //orderlist.Add(item.OrderNumber, (item.Name, item.Price, item.Quantity));
                    SalesByItem.ItemName.Add(item.Name);
                    SalesByItem.orderRevenue.Add(Int32.Parse(item.Price));
                    SalesByItem.Quantity.Add(item.Quantity);
                }
            }
            return SalesByItem;

        }
        [HttpGet("sale/invoice")]//This is so you can get the order with the quantity got and the revenue by invoice
        public async Task<ActionResult<SalesDto>> GetOrderItemByinvoice(ReportDto reportDto)
        {
            var results = await _firebaseDataContext.GetData(dir + reportDto.branchId);

            List<List<OrderItem>> temp = new List<List<OrderItem>>();
            SalesDto SalesByItem = new SalesDto();
            foreach (var item in results)
            {
                temp.Add((List<OrderItem>)item); // This weird little code is so that we make sure we get the correct type to loop through
            }
            foreach (var stork in temp)
            {
                foreach (var item in stork)
                {
                    string orderNo = item.OrderNumber.Substring(10);

                    if (orderNo==reportDto.invoice)
                    {
                        SalesByItem.orderRevenue.Add(Int32.Parse(item.Price));
                        SalesByItem.Quantity.Add(item.Quantity);
                    }
                }
            }
            return SalesByItem;
        }
        [HttpGet("sale/summary")]//This gets a flat out total in a period of time
        public async Task<ActionResult<SalesDto>> GetSummarySales(ReportDto reportDto)
        {
            List<List<OrderItem>> ordersgiven = new List<List<OrderItem>>();
            ordersgiven = await GetOrdersByDate(reportDto);


            SalesDto SalesByItem = new SalesDto();
            foreach (var order in ordersgiven)
            {
                foreach (var item in order)
                {
                    SalesByItem.SummaryTotal += Int32.Parse(item.Price);
                }
            }
            return SalesByItem;
        }
        [HttpGet("sales/paymentmethods")]// This get the balance for both type of payment asked for
        public async Task<ActionResult<SalesDto>> GetPaymentBalance(ReportDto reportDto)
        {
            List<List<OrderItem>> ordersgiven = new List<List<OrderItem>>();
            ordersgiven = await GetOrdersByDate(reportDto);

            Dictionary<string, (string, string)> paymentList=new Dictionary<string, (string, string)>();
            SalesDto SalesByItem = new SalesDto();
            foreach (var order in ordersgiven)
            {
                foreach (var item in order)
                {
                    paymentList.Add(item.OrderNumber, (item.Price,item.PaymentMethod));
                }
            }
            foreach (KeyValuePair<string, (string, string)> entry in paymentList)
            {
                if (entry.Value.Item2=="cash") SalesByItem.paymentMethodList["cash"] += Int32.Parse(entry.Value.Item1);

                if (entry.Value.Item2 == "bank") SalesByItem.paymentMethodList["bank"] += Int32.Parse(entry.Value.Item1);
            }

            return SalesByItem; //Kana if you change this ill honestly fight you cause it works properly
        }

    }
}
