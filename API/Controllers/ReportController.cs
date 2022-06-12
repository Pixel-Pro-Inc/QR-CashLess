using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _env;
        private readonly IReportServices _reportServices;

        public ReportController(IWebHostEnvironment env, IFirebaseServices firebaseService, IReportServices reportServices) :base(firebaseService)
        {
            _env = env;
            _reportServices = reportServices;
        }
        
        //REFACTOR: We might need a Report Service
        // TODO: Make a report Service

        [Authorize]        
        [HttpPost("sales/total")]
        public async Task<ActionResult<List<SalesDto>>> GetTotalSales(ReportDto reportDto)
        {
            //returns order number and amount earned
            List<List<OrderItem>> eligibleOrders = new List<List<OrderItem>>();

            eligibleOrders = await _reportServices.GetOrdersByDate(reportDto);

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

        [Authorize]
        [HttpPost("sales/item")] //This is the sales by item on a time period. It gives OrderNumber| item| weight| amount paid
        public async Task<ActionResult<List<Hashtable>>> GetSalesByItem(ReportDto reportDto)
        {
            List<List<OrderItem>> ordersgiven = new List<List<OrderItem>>();
            ordersgiven = await _reportServices.GetOrdersByDate(reportDto);

            List<Hashtable> hashtables = new List<Hashtable>();            


            // REFACTOR: Please please, there has to be another way
            foreach (var order in ordersgiven)
            {
                foreach (var item in order)
                {
                    if (_reportServices.FilterByCategory(reportDto) && _reportServices.FilterByName(reportDto))
                    {
                        if (reportDto.Category.ToUpper() == item.Category.ToUpper())
                            if (item.Name.ToUpper().Contains(reportDto.Name.ToUpper()) ||
                                item.SubCategory.ToUpper().Contains(reportDto.Name.ToUpper()))
                            {
                                hashtables.Add(GetHashtable(item));
                            }
                    }
                    else if (_reportServices.FilterByCategory(reportDto) && !_reportServices.FilterByName(reportDto))
                    {
                        if (reportDto.Category.ToUpper() == item.Category.ToUpper())
                        {
                            hashtables.Add(GetHashtable(item));
                        }
                    }
                    else if (!_reportServices.FilterByCategory(reportDto) && _reportServices.FilterByName(reportDto))
                    {
                        if (item.Name.ToUpper().Contains(reportDto.Name.ToUpper()) ||
                                item.SubCategory.ToUpper().Contains(reportDto.Name.ToUpper()))
                        {
                            hashtables.Add(GetHashtable(item));
                        }
                    }
                    else if (!_reportServices.FilterByCategory(reportDto) && !_reportServices.FilterByName(reportDto))
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

        
        [Authorize]
        [HttpPost("sales/invoice")]//This is so you can get the order with the quantity got and the revenue by invoice
        public async Task<ActionResult<List<List<OrderItem>>>> GetOrderItemByinvoice(ReportDto reportDto)
        {
            var results = await _reportServices.GetOrdersByDate(reportDto);

            List<List<OrderItem>> temp = new List<List<OrderItem>>();

            // REFACTOR: Please please, there has to be another way
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

        [Authorize]
        [HttpPost("sales/summary")]//This gets a flat out total in a period of time
        public async Task<ActionResult<SalesDto>> GetSummarySales(ReportDto reportDto)
        {
            //Gets the total Sales in the time period set in the ReportDto
            string total = FormatAmountString( await _reportServices.GetSalesAmountinTimePeriod(reportDto));

            SalesDto sales = new SalesDto()
            {
                OrderRevenue = total
            };

            return sales;
        }

        [Authorize]
        [HttpPost("sales/paymentmethods")]// This get the balance for both type of payment asked for
        public async Task<ActionResult<List<PaymentDto>>> GetPaymentBalance(ReportDto reportDto)
        {
            List<List<OrderItem>> ordersgiven = new List<List<OrderItem>>();
            ordersgiven = await _reportServices.GetOrdersByDate(reportDto);

            float cash = 0;
            float card = 0;
            float online = 0;

            // REFACTOR: Please please, there has to be another way
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

        [Authorize]
        [HttpGet("sales/thismonth/volume/{id}")]
        public async Task<ActionResult<Hashtable>> GetCurrentSales(string id)
        {
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                BranchId = id
            };
            // TODO: Put this in the Report Service

            // A tuple of orders of this month and the last
            var TwoMonthOrders = await _reportServices.GetTwoMonthOrders(reportDto);

            //Gets the specific timepspan between the start and end date for each month
            TimeSpan daysOrdering = reportDto.StartDate.Subtract(reportDto.EndDate);
            TimeSpan PreviousMpnthdaysOrdering = reportDto.StartDate.Subtract(reportDto.EndDate);

            // Gets the rate of orders per day
            float thisMonthratePerDay = TwoMonthOrders.ThisMonthorders.Count / daysOrdering.Days;
            float lastMonthratePerDay = TwoMonthOrders.LastMonthOrders.Count / PreviousMpnthdaysOrdering.Days;

            // Sets the variable names for the hashtable
            string metricname = "sales";
            int sales = TwoMonthOrders.ThisMonthorders.Count;
            float difference = thisMonthratePerDay - lastMonthratePerDay;
            string change = FormatAmountString(Math.Abs(thisMonthratePerDay / lastMonthratePerDay) * 100f);

            return _reportServices.GenerateReportHashtable(metricname, sales, difference, change);
        }

       


        [Authorize]
        [HttpGet("sales/thismonth/revenue/{id}")]
        public async Task<ActionResult<Hashtable>> GetCurrentRevenue(string id)
        {
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                BranchId = id
            };

            // TODO: Put this in the report Service

            var TwoMonthOrders = await _reportServices.GetTwoMonthOrders(reportDto);

            // Find the revenue of both months
            float revenue = _reportServices.FindRevenueinMonth(TwoMonthOrders.ThisMonthorders);
            float lastRevenue= _reportServices.FindRevenueinMonth(TwoMonthOrders.LastMonthOrders);

            float difference = revenue - lastRevenue;
            float absDifference = Math.Abs(difference);

            float percentageChange = (float)absDifference / (float)lastRevenue;

            return _reportServices.GenerateReportHashtable("revenue", revenue, absDifference, FormatAmountString(percentageChange* 100f));
        }

        

        [Authorize]
        [HttpGet("sales/thismonth/averagevolume/{id}")]
        public async Task<ActionResult<Hashtable>> GetCurrentAverageSales(string id)
        {
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            #region Date
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
            #endregion

            float avgOrdersCurrent = (float)orders.Count / (float)GetNumberOfDaysElapsed(orders);
            float avgOrdersPast = (float)lastMonthOrders.Count / (float)GetNumberOfDaysElapsed(lastMonthOrders);

            float difference = avgOrdersCurrent - avgOrdersPast;
            float absDifference = Math.Abs(difference);

            float percentageChange = absDifference / avgOrdersPast;

            return _reportServices.GenerateReportHashtable("averagesales", avgOrdersCurrent, absDifference, FormatAmountString(percentageChange * 100f));
        }

        [Authorize]
        [HttpGet("sales/thismonth/averagerevenue/{id}")]
        public async Task<ActionResult<Hashtable>> GetCurrentAverageRevenue(string id)
        {
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            #region Date
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
            #endregion

            float revCurrent = 0f;
            float revPast = 0f;

            foreach (var order in orders)
            {
                foreach (var orderItem in order)
                {
                    revCurrent += float.Parse(orderItem.Price);
                }                
            }

            foreach (var order in lastMonthOrders)
            {
                foreach (var orderItem in order)
                {
                    revPast += float.Parse(orderItem.Price);
                }
            }

            float avgRevOrdersCurrent = revCurrent/(float)orders.Count;
            float avgRevOrdersPast = revPast / (float)lastMonthOrders.Count;

            float difference = avgRevOrdersCurrent - avgRevOrdersPast;
            float absDifference = Math.Abs(difference);

            float percentageChange = (float)absDifference / (float)avgRevOrdersPast;

            return _reportServices.GenerateReportHashtable("averagerevenue", avgRevOrdersCurrent, absDifference, FormatAmountString(percentageChange * 100f));
        }

        [Authorize]
        [HttpGet("sales/thismonth/averageitems/{id}")]
        public async Task<ActionResult<Hashtable>> GetCurrentAverageItems(string id)
        {
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            #region Date
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
            #endregion

            float itemsCurrent = 0f;
            float itemsPast = 0f;

            foreach (var order in orders)
            {
                itemsCurrent += order.Count;
            }

            foreach (var order in lastMonthOrders)
            {
                itemsPast += order.Count;
            }

            float avgItemsOrdersCurrent = itemsCurrent / (float)orders.Count;
            float avgItemsOrdersPast = itemsPast / (float)lastMonthOrders.Count;

            float difference = avgItemsOrdersCurrent - avgItemsOrdersPast;
            float absDifference = Math.Abs(difference);

            float percentageChange = (float)absDifference / (float)avgItemsOrdersPast;

            return _reportServices.GenerateReportHashtable("averageitems", avgItemsOrdersCurrent, absDifference, FormatAmountString(percentageChange * 100f));
        }

        [Authorize]
        [HttpGet("sales/thismonth/ordersource/{id}")]
        public async Task<ActionResult<List<Hashtable>>> GetOrderSources(string id)
        {
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            #region Date
            reportDto.BranchId = id;
            reportDto.StartDate = new DateTime(reportDto.StartDate.Year, reportDto.StartDate.Month, 1);
            reportDto.EndDate = new DateTime(reportDto.EndDate.Year, reportDto.EndDate.Month, DateTime.DaysInMonth(reportDto.EndDate.Year, reportDto.EndDate.Month));

            var orders = await GetOrdersByDate(reportDto);
            #endregion

            List<Hashtable> hashtables = new List<Hashtable>();

            for (int i = 0; i < 4; i++)
            {
                hashtables.Add(new Hashtable());
            }

            int walkins = 0;
            int callins = 0;
            int onlines = 0;
            int deliveries = 0;

            foreach (var order in orders)
            {
                if(order[0].Reference.ToLower().Trim() == "till")
                    walkins++;

                if (order[0].Reference.ToLower().Trim() == "call")
                    callins++;

                if (order[0].Reference.ToLower().Trim().Contains("client"))
                    onlines++;

                if (order[0].Reference.ToLower().Trim() == "delivery")
                    deliveries++;
            }

            hashtables[0].Add("name", "Walk in");
            hashtables[0].Add("value", walkins);

            hashtables[1].Add("name", "Call in");
            hashtables[1].Add("value", callins);

            hashtables[2].Add("name", "Online");
            hashtables[2].Add("value", onlines);

            hashtables[3].Add("name", "Delivery");
            hashtables[3].Add("value", deliveries);

            return hashtables;
        }

        int GetNumberOfDaysElapsed(List<List<OrderItem>> orders)
        {
            List<string> elapsedDates = new List<string>();

            foreach (var order in orders)
            {
                if(!elapsedDates.Contains(order[0].OrderDateTime.ToShortDateString()))
                {
                    elapsedDates.Add(order[0].OrderDateTime.ToShortDateString());
                }
            }

            return elapsedDates.Count;
        }

        [Authorize]
        [HttpPost("excel/export-detailedsales")]
        public async Task<IActionResult> ExportDetailedSales(ReportDto reportDto)
        {
            List<List<OrderItem>> ordersgiven = new List<List<OrderItem>>();
            ordersgiven = await GetOrdersByDate(reportDto);

            List<List<OrderItem>> orderfiltered = new List<List<OrderItem>>();

            // REFACTOR: Theres a better way, have the total and remove instead of adding
            foreach (var order in ordersgiven)
            {
                orderfiltered.Add(new List<OrderItem>());
                foreach (var item in order)
                {
                    if (_reportServices.FilterByCategory(reportDto) && _reportServices.FilterByName(reportDto))
                    {
                        if (reportDto.Category.ToUpper() == item.Category.ToUpper())
                            if (item.Name.ToUpper().Contains(reportDto.Name.ToUpper()) ||
                                item.SubCategory.ToUpper().Contains(reportDto.Name.ToUpper()))
                            {
                                orderfiltered[orderfiltered.Count - 1].Add(item);
                            }
                    }
                    else if (_reportServices.FilterByCategory(reportDto) && !_reportServices.FilterByName(reportDto))
                    {
                        if (reportDto.Category.ToUpper() == item.Category.ToUpper())
                        {
                            orderfiltered[orderfiltered.Count - 1].Add(item);
                        }
                    }
                    else if (!_reportServices.FilterByCategory(reportDto) && _reportServices.FilterByName(reportDto))
                    {
                        if (item.Name.ToUpper().Contains(reportDto.Name.ToUpper()) ||
                                item.SubCategory.ToUpper().Contains(reportDto.Name.ToUpper()))
                        {
                            orderfiltered[orderfiltered.Count - 1].Add(item);
                        }
                    }
                    else if (!_reportServices.FilterByCategory(reportDto) && !_reportServices.FilterByName(reportDto))
                    {
                        orderfiltered[orderfiltered.Count - 1].Add(item);
                    }
                }

            }

            return await new ExcelController(_env, _firebaseServices).ExportData(orderfiltered);
        }

        [Authorize]
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

            return _reportServices.GenerateReportHashtable("revenue", revenue, absDifference, FormatAmountString(percentageChange * 100f));
        }
        public async Task<List<List<OrderItem>>> GetOrdersByDate(ReportDto reportDto)
        {
            List<List<OrderItem>> eligibleOrders = new List<List<OrderItem>>();
            List<List<OrderItem>> orders = await _firebaseServices.GetData<List<OrderItem>>(dir + reportDto.BranchId);

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
       
        // TODO: put this in the report service
        public async Task<List<List<OrderItem>>> GetAllOrdersByDate(ReportDto reportDto)
        {
            List<BranchDto> branches = new List<BranchDto>();

            var response = await _firebaseServices.GetBranchesFromDatabase();

            foreach (var item in response)
            {
                Branch branch = item;
                TimeSpan timeSpan = DateTime.UtcNow - item.LastActive;

                float x = (float)(timeSpan.TotalMinutes);

                BranchDto branchDto = new BranchDto()
                {
                    Id = item.Id,
                    Img = item.ImgUrl,
                    LastActive = x,
                    Location = item.Location,
                    Name = item.Name
                };

                branches.Add(branchDto);
            }

            List<List<OrderItem>> eligibleOrders = new List<List<OrderItem>>();
            List<List<OrderItem>> orders = new List<List<OrderItem>>();

            for (int i = 0; i < branches.Count; i++)
            {
                orders.Add( await _firebaseServices.GetOrders(branches[i].Id));
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

        [Authorize]
        [HttpPost("excel/export-totalsales")]
        public async Task<IActionResult> ExportTotalSales(ReportDto reportDto)
        {
            List<List<OrderItem>> ordersgiven = new List<List<OrderItem>>();
            ordersgiven = await GetOrdersByDate(reportDto);

            return await new ExcelController(_env, _firebaseServices).ExportTotalSalesData(ordersgiven);
        }

        [HttpPost("excel/export")] 
        public async Task<IActionResult> ExportIntercept(ReportDto reportDto)
        {
            List<List<OrderItem>> ordersgiven = new List<List<OrderItem>>();
            ordersgiven = await _reportServices.GetOrdersByDate(reportDto);

            List<List<OrderItem>> orderfiltered = new List<List<OrderItem>>();

            // REFACTOR: Theres a better way, have the total and remove instead of adding
            foreach (var order in ordersgiven)
            {
                orderfiltered.Add(new List<OrderItem>());
                foreach (var item in order)
                {
                    if (_reportServices.FilterByCategory(reportDto) && _reportServices.FilterByName(reportDto))
                    {
                        if (reportDto.Category.ToUpper() == item.Category.ToUpper())
                            if (reportDto.Name.ToUpper() == item.Name.ToUpper())
                            {
                                orderfiltered[orderfiltered.Count-1].Add(item);
                            }
                    }
                    else if (_reportServices.FilterByCategory(reportDto) && !_reportServices.FilterByName(reportDto))
                    {
                        if (reportDto.Category.ToUpper() == item.Category.ToUpper())
                        {
                            orderfiltered[orderfiltered.Count - 1].Add(item);
                        }
                    }
                    else if (!_reportServices.FilterByCategory(reportDto) && _reportServices.FilterByName(reportDto))
                    {
                        if (reportDto.Name.ToUpper() == item.Name.ToUpper())
                        {
                            orderfiltered[orderfiltered.Count - 1].Add(item);
                        }
                    }
                    else if (!_reportServices.FilterByCategory(reportDto) && !_reportServices.FilterByName(reportDto))
                    {
                        orderfiltered[orderfiltered.Count - 1].Add(item);
                    }
                }

                if (orderfiltered[orderfiltered.Count - 1].Count==0)
                    orderfiltered[orderfiltered.Count - 1].RemoveAt(orderfiltered.Count - 1);

            }

            return await new ExcelController(_env, _firebaseServices).ExportData(orderfiltered);

        }

        // TODO: put this in the report service
        string GetOrderNumber(string OrderNumber)
        {
            return OrderNumber.Substring(OrderNumber.IndexOf('_') + 1, 4);
        }
        // TODO: put this in the report service
        string FormatAmountString(float amount) // format 1,000,000.00
        {
            return String.Format("{0:n}", amount);
        }

    }
}