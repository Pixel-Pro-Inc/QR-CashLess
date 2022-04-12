using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ReportController : BaseApiController
    {
        private readonly string dir = "CompletedOrders/";

        public ReportController():base(){ }
        
        [HttpPost("sales/total")]
        public async Task<ActionResult<List<SalesDto>>> GetTotalSales(ReportDto reportDto)
        {
            //returns order number and amount earned
            List<List<OrderItem>> eligibleOrders = new List<List<OrderItem>>();

            eligibleOrders = await GetOrdersByDate(reportDto);

            List<float> totals = new List<float>();
            List<string> OrderNumbers = new List<string>();

            for (int i = 0; i < eligibleOrders.Count; i++)
            {
                OrderNumbers.Add(GetOrderNumber(eligibleOrders[i][0].OrderNumber));
            }

            for (int i = 0; i < eligibleOrders.Count; i++)
            {
                totals.Add(0);

                foreach (var item in eligibleOrders[i])
                {
                    totals[i] += float.Parse(item.Price);
                }
            }

            List<SalesDto> totalSalesDto = new List<SalesDto>();

            for (int i = 0; i < totals.Count; i++)
            {
                totalSalesDto.Add(new SalesDto() { OrderNumber = OrderNumbers[i], OrderRevenue = FormatAmountString(totals[i]) });
            }

            return totalSalesDto;
        }        

        [HttpPost("sales/item")] //This is the sales by item on a time period. It gives OrderNumber| item| weight| amount paid
        public async Task<ActionResult<List<Hashtable>>> GetSalesByItem(ReportDto reportDto)
        {
            List<List<OrderItem>> ordersgiven = new List<List<OrderItem>>();
            ordersgiven = await GetOrdersByDate(reportDto);

            List<Hashtable> hashtables = new List<Hashtable>();            

            foreach (var order in ordersgiven)
            {
                foreach (var item in order)
                {
                    if (FilterByCategory(reportDto) && FilterByName(reportDto))
                    {
                        if (reportDto.Category.ToUpper() == item.Category.ToUpper())
                            if (reportDto.Name.ToUpper() == item.Name.ToUpper())
                            {
                                hashtables.Add(GetHashtable(item));
                            }
                    }
                    else if (FilterByCategory(reportDto) && !FilterByName(reportDto))
                    {
                        if (reportDto.Category.ToUpper() == item.Category.ToUpper())
                        {
                            hashtables.Add(GetHashtable(item));
                        }
                    }
                    else if (!FilterByCategory(reportDto) && FilterByName(reportDto))
                    {
                        if (reportDto.Name.ToUpper() == item.Name.ToUpper())
                        {
                            hashtables.Add(GetHashtable(item));
                        }
                    }
                    else if (!FilterByCategory(reportDto) && !FilterByName(reportDto))
                    {
                        hashtables.Add(GetHashtable(item));
                    }
                }
            }

            return hashtables;
        }

        // TRACK: This needs to be studied. I have an idea of what it is doing but why did he chose this, or to accomplish what
        Hashtable GetHashtable(OrderItem item) //Formats data into a form for the angular client to understand
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("orderNumber", GetOrderNumber(item.OrderNumber));
            hashtable.Add("itemName", item.Name);
            hashtable.Add("orderRevenue", FormatAmountString(float.Parse(item.Price)));
            hashtable.Add("quantity", item.Quantity);

            if (!string.IsNullOrEmpty(item.Weight))
            {
                bool condition = item.Weight.Contains(' ');

                if (condition)
                {
                    string weight = item.Weight.Substring(0, item.Weight.IndexOf(' ') + 1);
                    string newWeight = (float.Parse(weight) * (float)item.Quantity).ToString();

                    hashtable.Add("weight", newWeight);
                    return hashtable;
                }

                hashtable.Add("weight", (float.Parse(item.Weight) * (float)item.Quantity).ToString());
                return hashtable;
            }

            hashtable.Add("weight", "-");
            return hashtable;
        } 

        bool FilterByCategory(ReportDto reportDto)
        {
            if (reportDto.Category != null)
                if (reportDto.Category != "None" && reportDto.Category != "")
                {
                    return true;
                }

            return false;           
        }

        bool FilterByName(ReportDto reportDto)
        {
            if (reportDto.Name != null)
                if (reportDto.Name !="")
                {
                    return true;
                }

            return false;
        }

        [HttpPost("sales/invoice")]//This is so you can get the order with the quantity got and the revenue by invoice
        public async Task<ActionResult<List<List<OrderItem>>>> GetOrderItemByinvoice(ReportDto reportDto)
        {
            var results = await GetOrdersByDate(reportDto);

            List<List<OrderItem>> temp = new List<List<OrderItem>>();

            foreach (var item in results)
            {
                string x = item[0].OrderNumber;

                if(reportDto.Invoice != null)
                {
                    if (reportDto.Invoice.Length > 0)
                    {
                        if (x.Substring(x.IndexOf('_') + 1, 4) == reportDto.Invoice)
                        {
                            item[0].OrderNumber = reportDto.Invoice;
                            item[0].Description = x.Substring(0, 10).Replace('-', '/');
                            temp.Add(GetWeightByQuantity(item));
                        }
                    }
                    else
                    {
                        item[0].OrderNumber = GetOrderNumber(item[0].OrderNumber);
                        item[0].Description = x.Substring(0, 10).Replace('-', '/');
                        temp.Add(GetWeightByQuantity(item));
                    }
                }
                else
                {
                    item[0].OrderNumber = GetOrderNumber(item[0].OrderNumber);
                    item[0].Description = x.Substring(0, 10).Replace('-', '/');
                    temp.Add(GetWeightByQuantity(item));
                }
            }

            foreach (var item in temp)
            {
                foreach (var x in item)
                {
                    x.Price = FormatAmountString(float.Parse(x.Price));
                }
            }

            return temp;
        }

        List<OrderItem> GetWeightByQuantity(List<OrderItem> items)
        {
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.Weight))
                {
                    bool condition = item.Weight.Contains(' ');

                    if (condition)
                    {
                        string weight = item.Weight.Substring(0, item.Weight.IndexOf(' ') + 1);
                        item.Weight = (float.Parse(weight) * (float)item.Quantity).ToString();
                        continue;
                    }

                    item.Weight = (float.Parse(item.Weight) * (float)item.Quantity).ToString();
                    continue;
                }

                item.Weight = "-";
            }

            return items;
        }

        [HttpPost("sales/summary")]//This gets a flat out total in a period of time
        public async Task<ActionResult<SalesDto>> GetSummarySales(ReportDto reportDto)
        {
            List<List<OrderItem>> ordersgiven = new List<List<OrderItem>>();
            ordersgiven = await GetOrdersByDate(reportDto);

            SalesDto SalesByItem = new SalesDto();
            foreach (var order in ordersgiven)
            {
                foreach (var item in order)
                {
                    SalesByItem.SummaryTotal += float.Parse(item.Price);
                }
            }

            string total = FormatAmountString(SalesByItem.SummaryTotal);

            SalesDto sales = new SalesDto()
            {
                OrderRevenue = total
            };

            return sales;
        }
        [HttpPost("sales/paymentmethods")]// This get the balance for both type of payment asked for
        public async Task<ActionResult<List<PaymentDto>>> GetPaymentBalance(ReportDto reportDto)
        {
            List<List<OrderItem>> ordersgiven = new List<List<OrderItem>>();
            ordersgiven = await GetOrdersByDate(reportDto);

            float cash = 0;
            float card = 0;
            float online = 0;

            foreach (var item in ordersgiven)
            {
                foreach (var order in item)
                {
                    if(order.PaymentMethod != null)
                    {
                        if (order.PaymentMethod.ToUpper() == "CASH")
                        {
                            cash += float.Parse(order.Price);
                        }

                        if (order.PaymentMethod.ToUpper() == "CARD")
                        {
                            card += float.Parse(order.Price);
                        }

                        if (order.PaymentMethod.ToUpper() == "ONLINE")
                        {
                            online += float.Parse(order.Price);
                        }
                    }                    
                }
            }

            List<PaymentDto> payments = new List<PaymentDto>();

            PaymentDto paymentDto = new PaymentDto()
            {
                Method = "Cash",
                Amount = FormatAmountString(cash)
            };

            payments.Add(paymentDto);

            paymentDto = new PaymentDto()
            {
                Method = "Card",
                Amount = FormatAmountString(card)
            };

            payments.Add(paymentDto);

            paymentDto = new PaymentDto()
            {
                Method = "Online",
                Amount = FormatAmountString(online)
            };

            payments.Add(paymentDto);


            return payments; //Kana if you change this ill honestly fight you cause it works properly
        }

        [HttpGet("sales/thismonth/volume/{id}")]
        public async Task<ActionResult<Hashtable>> GetCurrentSales(string id)
        {
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            reportDto.BranchId = id;
            reportDto.StartDate = new DateTime(reportDto.StartDate.Year, reportDto.StartDate.Month, 1);
            reportDto.EndDate = new DateTime(reportDto.EndDate.Year, reportDto.EndDate.Month, DateTime.DaysInMonth(reportDto.EndDate.Year, reportDto.EndDate.Month));

            var orders = await GetOrdersByDate(reportDto);

            reportDto.BranchId = id;
            reportDto.StartDate = new DateTime(reportDto.StartDate.Year, reportDto.StartDate.Month, 1);
            reportDto.StartDate = reportDto.StartDate.AddMonths(-1);

            reportDto.EndDate = new DateTime(reportDto.EndDate.Year, reportDto.EndDate.Month, DateTime.DaysInMonth(reportDto.EndDate.Year, reportDto.EndDate.Month));
            reportDto.EndDate = reportDto.EndDate.AddMonths(-1);

            var lastMonthOrders = await GetOrdersByDate(reportDto);

            int difference = orders.Count - lastMonthOrders.Count;
            int absDifference = Math.Abs(difference);

            float percentageChange = (float)absDifference / (float)lastMonthOrders.Count;

            Hashtable hashtable = new Hashtable();
            hashtable.Add("sales", orders.Count);

            string change = FormatAmountString(difference < 0 ? percentageChange * -100f : percentageChange * 100f);
            hashtable.Add("change", change);

            hashtable.Add("positive", difference < 0 ? false : true);

            return hashtable;
        }
        [HttpGet("sales/thismonth/revenue/{id}")]
        public async Task<ActionResult<Hashtable>> GetCurrentRevenue(string id)
        {
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            reportDto.BranchId = id;
            reportDto.StartDate = new DateTime(reportDto.StartDate.Year, reportDto.StartDate.Month, 1);
            reportDto.EndDate = new DateTime(reportDto.EndDate.Year, reportDto.EndDate.Month, DateTime.DaysInMonth(reportDto.EndDate.Year, reportDto.EndDate.Month));

            var orders = await GetOrdersByDate(reportDto);
            float revenue = 0f;
            foreach (var item in orders)
            {
                foreach (var order in item)
                {
                    revenue += float.Parse(order.Price);
                }
            }

            reportDto.BranchId = id;
            reportDto.StartDate = new DateTime(reportDto.StartDate.Year, reportDto.StartDate.Month, 1);
            reportDto.StartDate = reportDto.StartDate.AddMonths(-1);

            reportDto.EndDate = new DateTime(reportDto.EndDate.Year, reportDto.EndDate.Month, DateTime.DaysInMonth(reportDto.EndDate.Year, reportDto.EndDate.Month));
            reportDto.EndDate = reportDto.EndDate.AddMonths(-1);

            var lastMonthOrders = await GetOrdersByDate(reportDto);
            float lastRevenue = 0f;
            foreach (var item in lastMonthOrders)
            {
                foreach (var order in item)
                {
                    lastRevenue += float.Parse(order.Price);
                }
            }

            float difference = revenue - lastRevenue;
            float absDifference = Math.Abs(difference);

            float percentageChange = (float)absDifference / (float)lastRevenue;

            Hashtable hashtable = new Hashtable();
            hashtable.Add("revenue", FormatAmountString(revenue));

            string change = FormatAmountString(difference < 0 ? percentageChange * -100f : percentageChange * 100f);
            hashtable.Add("change", change);

            hashtable.Add("positive", difference < 0 ? false : true);

            return hashtable;
        }
        [HttpGet("sales/thismonth/allrevenue")]
        public async Task<ActionResult<Hashtable>> GetAllCurrentRevenue()
        {
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            reportDto.StartDate = new DateTime(reportDto.StartDate.Year, reportDto.StartDate.Month, 1);
            reportDto.EndDate = new DateTime(reportDto.EndDate.Year, reportDto.EndDate.Month, DateTime.DaysInMonth(reportDto.EndDate.Year, reportDto.EndDate.Month));

            var orders = await GetAllOrdersByDate(reportDto);
            float revenue = 0f;
            foreach (var item in orders)
            {
                foreach (var order in item)
                {
                    revenue += float.Parse(order.Price);
                }
            }

            reportDto.StartDate = new DateTime(reportDto.StartDate.Year, reportDto.StartDate.Month, 1);
            reportDto.StartDate = reportDto.StartDate.AddMonths(-1);

            reportDto.EndDate = new DateTime(reportDto.EndDate.Year, reportDto.EndDate.Month, DateTime.DaysInMonth(reportDto.EndDate.Year, reportDto.EndDate.Month));
            reportDto.EndDate = reportDto.EndDate.AddMonths(-1);

            var lastMonthOrders = await GetAllOrdersByDate(reportDto);
            float lastRevenue = 0f;
            foreach (var item in lastMonthOrders)
            {
                foreach (var order in item)
                {
                    lastRevenue += float.Parse(order.Price);
                }
            }

            float difference = revenue - lastRevenue;
            float absDifference = Math.Abs(difference);

            float percentageChange = (float)absDifference / (float)lastRevenue;

            Hashtable hashtable = new Hashtable();
            hashtable.Add("revenue", FormatAmountString(revenue));

            string change = FormatAmountString(difference < 0 ? percentageChange * -100f : percentageChange * 100f);
            hashtable.Add("change", change);

            hashtable.Add("positive", difference < 0 ? false : true);

            return hashtable;
        }
        public async Task<List<List<OrderItem>>> GetOrdersByDate(ReportDto reportDto)
        {
            List<List<OrderItem>> eligibleOrders = new List<List<OrderItem>>();
            List<List<OrderItem>> orders = await _firebaseDataContext.GetData<List<OrderItem>>(dir + reportDto.BranchId);

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

                    DateTime startDate = reportDto.StartDate;
                    DateTime endDate = reportDto.EndDate;

                    if (orderTime >= startDate && orderTime <= endDate)
                    {
                        eligibleOrders.Add(order);
                        i = order.Count;
                    }
                }
            }

            return eligibleOrders;
        }
        public async Task<List<List<OrderItem>>> GetAllOrdersByDate(ReportDto reportDto)
        {
            List<BranchDto> branches = new List<BranchDto>();

            var response = await _firebaseDataContext.GetData<Branch>("Branch");

            foreach (var item in response)
            {
                Branch branch = item;

                TimeSpan timeSpan = DateTime.UtcNow - branch.LastActive;

                float x = (float)(timeSpan.TotalMinutes);

                BranchDto branchDto = new BranchDto()
                {
                    Id = branch.Id,
                    Img = branch.ImgUrl,
                    LastActive = x,
                    Location = branch.Location,
                    Name = branch.Name
                };

                branches.Add(branchDto);
            }

            List<List<OrderItem>> eligibleOrders = new List<List<OrderItem>>();
            List<List<OrderItem>> orders = new List<List<OrderItem>>();

            for (int i = 0; i < branches.Count; i++)
            {
                var result = await _firebaseDataContext.GetData<List<OrderItem>>(dir + branches[i].Id);

                foreach (var item in result)
                {
                    var pain = item;
                    orders.Add(pain);
                }
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

                    DateTime startDate = reportDto.StartDate;
                    DateTime endDate = reportDto.EndDate;

                    if (orderTime >= startDate && orderTime <= endDate)
                    {
                        eligibleOrders.Add(order);
                        i = order.Count;
                    }
                }
            }

            return eligibleOrders;
        }
        string GetOrderNumber(string OrderNumber)
        {
            return OrderNumber.Substring(OrderNumber.IndexOf('_') + 1, 4);
        }
        string FormatAmountString(float amount) // format 1,000,000.00
        {
            return String.Format("{0:n}", amount);
        }
    }
}