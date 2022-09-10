using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using RodizioSmartKernel.Entities.Aggregates;
using RodizioSmartKernel.Entities;
using API.Application.Interfaces;
using API.Application.DTOs;
using API.Application.Helpers;
using API.Application.Extensions;

namespace API.Infrastructure.Controllers
{
    public class ReportController : BaseApiController
    {
        private readonly IReportServices _reportServices;
        private readonly IExcelService _excelService;

        public ReportController(IFirebaseServices firebaseService, IReportServices reportServices, IExcelService excelService) :base(firebaseService)
        {
            _reportServices = reportServices;
            _excelService = excelService;
        }
        
        
        [Authorize]        
        [HttpPost("sales/total")]
        public async Task<ActionResult<List<SalesDto>>> GetTotalSales(ReportDto reportDto)
        {
            //returns order number and amount earned
            List<Order> eligibleOrders = await _reportServices.GetOrdersByDate(reportDto);

            List<float> totals = new List<float>();
            List<string> OrderNumbers = new List<string>();

            for (int i = 0; i < eligibleOrders.Count; i++)
            {
                OrderNumbers.Add(eligibleOrders[i][0].OrderNumber.OrderNumber());
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
                totalSalesDto.Add(new SalesDto() { OrderNumber = OrderNumbers[i], OrderRevenue = totals[i].AmountToString() });
            }

            return totalSalesDto;
        }        
        [Authorize]
        [HttpPost("sales/item")] //This is the sales by item on a time period. It gives OrderNumber| item| weight| amount paid
        public async Task<ActionResult<List<Hashtable>>> GetSalesByItem(ReportDto reportDto)
        {
            List<Order> ordersgiven = await _reportServices.GetOrdersByDate(reportDto);

            List<Hashtable> hashtables = new List<Hashtable>();
            foreach (var order in ordersgiven)
            {
                foreach (var item in order)
                {
                    hashtables.Add( (Hashtable)_reportServices.OrderItemFilter<Hashtable>(reportDto.Category, item));
                }
            }

            return hashtables;
        }
        [Authorize]
        [HttpPost("sales/invoice")]//This is so you can get the order with the quantity got and the revenue by invoice
        public async Task<ActionResult<List<Order>>> GetOrderItemByinvoice(ReportDto reportDto)
        {
            var results = await _reportServices.GetOrdersByDate(reportDto);

            List<Order> temp = new List<Order>();

            // REFACTOR: Please please, there has to be another way
            foreach (var item in results)
            {
                string x = item[0].OrderNumber;

                // Checks if the invoiceNumber is the same as the orderNumber and give the Ordernumber to itself or formats it correctly
                item[0].OrderNumber = x.Substring(x.IndexOf('_') + 1, 4) == reportDto.Invoice ?
                    reportDto.Invoice : item[0].OrderNumber = item[0].OrderNumber.OrderNumber();
                item[0].Description = x.Substring(0, 10).Replace('-', '/');
                temp.Add(_reportServices.GetWeightByQuantity(item));
            }

            foreach (var item in temp)
            {
                foreach (var x in item)
                {
                    x.Price = float.Parse(x.Price).AmountToString();
                }
            }

            return temp;
        }
        [Authorize]
        [HttpPost("sales/summary")]//This gets a flat out total in a period of time
        public async Task<ActionResult<SalesDto>> GetSummarySales(ReportDto reportDto)
        {
            //Gets the total Sales in the time period set in the ReportDto
            string total = (await _reportServices.GetSalesAmountinTimePeriod(reportDto)).AmountToString();

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
            List<Order> ordersgiven = new List<Order>();
            ordersgiven = await _reportServices.GetOrdersByDate(reportDto);

            float cash = 0;
            float card = 0;
            float online = 0;

            string[] paymentTypes = new string[3] { "Cash", "Card", "Online" };
            float[] amountList = new float[3] { cash, card, online };

            return _reportServices.GetpaymentTypeBalances(ordersgiven,paymentTypes, amountList);

        }
        [Authorize]
        [HttpGet("sales/thismonth/volume/{id}")]
        public async Task<ActionResult<Hashtable>> GetCurrentSales(string id)
        {
            // Time period from the first day of the month to current time
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now.FirstDayOfMonth(),
                EndDate = DateTime.Now,
                BranchId = id
            };

            // NOTE: We leave things as they are here cause it is a unique logic

            // A tuple of orders of this month and the last
            var TwoMonthOrders = await _reportServices.GetTwoMonthOrders(reportDto);

            //Gets the specific timepspan between the start and end date for each month
            TimeSpan daysOrdering = reportDto.StartDate.Subtract(reportDto.EndDate);
            TimeSpan PreviousMpnthdaysOrdering = reportDto.StartDate.AddMonths(-1).Subtract(reportDto.EndDate.AddMonths(-1));

            // Gets the rate of orders per day
            float thisMonthratePerDay = TwoMonthOrders.ThisMonthorders.Count / daysOrdering.Days;
            float lastMonthratePerDay = TwoMonthOrders.LastMonthOrders.Count / PreviousMpnthdaysOrdering.Days;

            // Sets the variable names for the hashtable
            string metricname = "sales";
            int sales = TwoMonthOrders.ThisMonthorders.Count;
            float difference = thisMonthratePerDay - lastMonthratePerDay;
            string change = (Math.Abs(thisMonthratePerDay / lastMonthratePerDay) * 100f).AmountToString();

            return _reportServices.GenerateReportHashtable(metricname, sales, difference, change);
        }
        [Authorize]
        [HttpGet("sales/thismonth/revenue/{id}")]
        public async Task<ActionResult<Hashtable>> GetCurrentRevenue(string id)
        {
            // Time period from the first day of the month to current time
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now.FirstDayOfMonth(),
                EndDate = DateTime.Now,
                BranchId = id
            };

            var TwoMonthRevenue = await _reportServices.GetTwoMonthsRevenue(reportDto);

            float difference = TwoMonthRevenue.ThisMonthRevenue - TwoMonthRevenue.LastMonthRevenue;
            float absDifference = Math.Abs(difference);

            float percentageChange = (float)absDifference / (float)TwoMonthRevenue.LastMonthRevenue;

            return _reportServices.GenerateReportHashtable("revenue", TwoMonthRevenue.ThisMonthRevenue, absDifference, (percentageChange * 100f).AmountToString());
        }
        [Authorize]
        [HttpGet("sales/thismonth/averagevolume/{id}")]
        public async Task<ActionResult<Hashtable>> GetCurrentAverageSales(string id)
        {
            // Time period from the first day of the month to current time
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now.FirstDayOfMonth(),
                EndDate = DateTime.Now,
                BranchId = id
            };

            var TwoMonthOrders = await _reportServices.GetTwoMonthOrders(reportDto);

            float avgOrdersCurrent = (float)TwoMonthOrders.ThisMonthorders.Count /_reportServices.GetNumberOfDaysElapsed(TwoMonthOrders.ThisMonthorders);
            float avgOrdersPast = (float)TwoMonthOrders.LastMonthOrders.Count /_reportServices.GetNumberOfDaysElapsed(TwoMonthOrders.LastMonthOrders);

            float difference = avgOrdersCurrent - avgOrdersPast;
            float absDifference = Math.Abs(difference);

            float percentageChange = absDifference / avgOrdersPast;

            return _reportServices.GenerateReportHashtable("averagesales", avgOrdersCurrent, absDifference, (percentageChange * 100f).AmountToString());
        }
        [Authorize]
        [HttpGet("sales/thismonth/averagerevenue/{id}")]
        public async Task<ActionResult<Hashtable>> GetCurrentAverageRevenue(string id)
        {
            // Time period from the first day of the month to current time
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now.FirstDayOfMonth(),
                EndDate = DateTime.Now,
                BranchId = id
            };

            // NOTE: I left it like this cause we need the order count and not just the revenue

            var TwoMonthOrders = await _reportServices.GetTwoMonthOrders(reportDto);

            // Find the revenue of both months
            float revCurrent = _reportServices.FindRevenueinMonth(TwoMonthOrders.ThisMonthorders);
            float revPast = _reportServices.FindRevenueinMonth(TwoMonthOrders.LastMonthOrders);

            // This is what separates The average from the current
            float avgRevOrdersCurrent = revCurrent/(float)TwoMonthOrders.ThisMonthorders.Count;
            float avgRevOrdersPast = revPast / (float)TwoMonthOrders.LastMonthOrders.Count;

            float difference = avgRevOrdersCurrent - avgRevOrdersPast;
            float absDifference = Math.Abs(difference);

            float percentageChange = (float)absDifference / (float)avgRevOrdersPast;

            return _reportServices.GenerateReportHashtable("averagerevenue", avgRevOrdersCurrent, absDifference, (percentageChange * 100f).AmountToString());
        }
        [Authorize]
        [HttpGet("sales/thismonth/averageitems/{id}")]
        public async Task<ActionResult<Hashtable>> GetCurrentAverageItems(string id)
        {
            // Time period from the first day of the month to current time
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now.FirstDayOfMonth(),
                EndDate = DateTime.Now,
                BranchId = id
            };

            var TwoMonthOrders = await _reportServices.GetTwoMonthOrders(reportDto);

            // NOTE: I left this logic cause it is unique
            float itemsCurrent = 0f;
            float itemsPast = 0f;

            // Accumulates the number of order items in the months
            foreach (var order in TwoMonthOrders.ThisMonthorders)
            {
                itemsCurrent += order.Count;
            }
            foreach (var order in TwoMonthOrders.LastMonthOrders)
            {
                itemsPast += order.Count;
            }

            float avgItemsOrdersCurrent = itemsCurrent / (float)TwoMonthOrders.ThisMonthorders.Count;
            float avgItemsOrdersPast = itemsPast / (float)TwoMonthOrders.LastMonthOrders.Count;

            float difference = avgItemsOrdersCurrent - avgItemsOrdersPast;
            float absDifference = Math.Abs(difference);

            float percentageChange = (float)absDifference / (float)avgItemsOrdersPast;

            return _reportServices.GenerateReportHashtable("averageitems", avgItemsOrdersCurrent, absDifference, (percentageChange * 100f).AmountToString());
        }
        [Authorize]
        [HttpGet("sales/thismonth/ordersource/{id}")]
        public async Task<ActionResult<List<Hashtable>>> GetOrderSources(string id)
        {
            // Time period from the first day of the month to current time
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now.FirstDayOfMonth(),
                EndDate = DateTime.Now,
                BranchId = id
            };

            var TwoMonthOrders = await _reportServices.GetTwoMonthOrders(reportDto);

            int walkins = 0;
            int callins = 0;
            int onlines = 0;
            int deliveries = 0;

            // Left this logic cause I couldn't think of a more effecient pattern and the logic isn't found anywhere else
            foreach (var order in TwoMonthOrders.ThisMonthorders)
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

            return _reportServices.GenerateSourcesHashtable(new string[4] { "Walk in", "Call in", "Online", "Delivery" }, new int[4] { walkins, callins, onlines , deliveries });

        }
        /// <summary>
        /// This method gets the revenue of all the branches
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("sales/thismonth/allrevenue")]
        public async Task<ActionResult<Hashtable>> GetAllCurrentRevenue()
        {
            ReportDto reportDto = new ReportDto()
            {
                StartDate = DateTime.Now.FirstDayOfMonth(),
                EndDate = DateTime.Now
            };

            var TwoMonthRevenue = await _reportServices.GetTwoMonthsRevenue(reportDto);

            float difference = TwoMonthRevenue.ThisMonthRevenue - TwoMonthRevenue.LastMonthRevenue;
            float absDifference = Math.Abs(difference);

            float percentageChange = (float)absDifference / (float)TwoMonthRevenue.LastMonthRevenue;

            return _reportServices.GenerateReportHashtable("revenue", TwoMonthRevenue.ThisMonthRevenue, absDifference, (percentageChange * 100f).AmountToString());
        }


        [HttpGet("excel/export/{branchId}")]
        public async Task<IActionResult> ExportData(string branchId)
        {
            try
            {
                return await _excelService.ExportDataFromDatabase(branchId);
            }
            catch (NullReferenceException)
            {
                return BadRequest("There is no data to export");
                throw;
            }
        }
        [Authorize]
        [HttpPost("excel/export-detailedsales")]
        public async Task<IActionResult> ExportDetailedSales(ReportDto reportDto)
        {
            List<Order> ordersgiven = await _reportServices.GetOrdersByDate(reportDto);

            List<Order> orderfiltered = new List<Order>();

            // REFACTOR: Theres a better way, have the total and remove instead of adding
            foreach (var order in ordersgiven)
            {
                orderfiltered.Add(new Order());
                foreach (var item in order)
                {
                    orderfiltered[orderfiltered.Count - 1].Add((OrderItem)_reportServices.OrderItemFilter<OrderItem>(reportDto.Category, item));
                }

            }

            return await _excelService.ExportDataFromDatabase(orderfiltered);
        }
        [Authorize]
        [HttpPost("excel/export-totalsales")]
        public async Task<IActionResult> ExportTotalSales(ReportDto reportDto)
        {
            List<Order> ordersgiven = new List<Order>();
            ordersgiven = await _reportServices.GetOrdersByDate(reportDto);

            return await _excelService.ExportDataFromDatabase(ordersgiven);
        }
        [HttpPost("excel/export")] 
        public async Task<IActionResult> ExportIntercept(ReportDto reportDto)
        {
            List<Order> ordersgiven = await _reportServices.GetOrdersByDate(reportDto);

            List<Order> orderfiltered = new List<Order>();

            // REFACTOR: Theres a better way, have the total and remove instead of adding
            foreach (var order in ordersgiven)
            {
                orderfiltered.Add(new Order());
                foreach (var item in order)
                {
                    orderfiltered[orderfiltered.Count - 1].Add((OrderItem)_reportServices.OrderItemFilter<OrderItem>(reportDto.Category, item));
                }

                if (orderfiltered[orderfiltered.Count - 1].Count == 0)
                    orderfiltered[orderfiltered.Count - 1].RemoveAt(orderfiltered.Count - 1);
            }

            return await _excelService.ExportDataFromDatabase(orderfiltered);

        }


    }
}