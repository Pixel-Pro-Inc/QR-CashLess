using API.DTOs;
using API.Exceptions;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RodizioSmartKernel.Entities;
using RodizioSmartKernel.Entities.Aggregates;
using RodizioSmartKernel.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class ReportServices : IReportServices
    {
        private readonly IFirebaseServices _firebaseServices;
        private readonly IMapper _mapper;


        public ReportServices(IFirebaseServices firebaseServices, IMapper mapper)
        {
            _firebaseServices = firebaseServices;
            _mapper = mapper;
        }

        // TODO: Have a cache property so that you don't have to call the database everytime you want a value

        // Sales and Revenue
        public async Task<float> GetSalesAmountinTimePeriod(ReportDto reportDto)
        {
            List<Order> ordersgiven = new List<Order>();
            ordersgiven = await GetOrdersByDate(reportDto);

            SalesDto SalesByItem = new SalesDto();
            foreach (var order in ordersgiven)
            {
                foreach (var item in order)
                {
                    SalesByItem.SummaryTotal += float.Parse(item.Price);
                }
            }

            return SalesByItem.SummaryTotal;
        }
        public float FindRevenueinMonth(List<Order> orders)
        {
            float revenue = 0f;
            foreach (var item in orders)
            {
                foreach (var order in item)
                {
                    revenue += float.Parse(order.Price);
                }
            }

            return revenue;
        }
        public async Task<(float ThisMonthRevenue, float LastMonthRevenue)> GetTwoMonthsRevenue(ReportDto reportDto)
        {
            var TwoMonthOrders = await GetTwoMonthOrders(reportDto);

            // Find the revenue of both months
            float revenue = FindRevenueinMonth(TwoMonthOrders.ThisMonthorders);
            float lastRevenue = FindRevenueinMonth(TwoMonthOrders.LastMonthOrders);

            return (revenue, lastRevenue);
        }
        public List<PaymentDto> GetpaymentTypeBalances(List<Order> ordersgiven, string[] paymentTypes, float[] amountList)
        {
            List<PaymentDto> paymentDtos = new List<PaymentDto>();

            // Finds the amount acquired for each type of payment
            foreach (var order in ordersgiven)
            {
                for (int i = 0; i < paymentTypes.Length; i++)
                {
                    // We don't need to check the orderitems cause they were all paid the same way
                    if (order[0].PaymentMethod != null && order[0].PaymentMethod.ToUpper() == paymentTypes[i].ToUpper())
                        amountList[i] += float.Parse(order[0].Price);
                }

            }

            // Generates the paymentDto based on the amount calculated for each paymenttype
            for (int i = 0; i < paymentTypes.Length; i++)
            {
                paymentDtos.Add(
                    new PaymentDto()
                    {
                        Method = paymentTypes[i],
                        Amount = Format.AmountToString(amountList[i])
                    });
            }
            return paymentDtos;
        }

        // Date and Time
        public int GetNumberOfDaysElapsed(List<Order> orders)
        {
            List<string> elapsedDates = new List<string>();

            foreach (var order in orders)
            {
                if (!elapsedDates.Contains(order[0].OrderDateTime.ToShortDateString()))
                {
                    elapsedDates.Add(order[0].OrderDateTime.ToShortDateString());
                }
            }

            return elapsedDates.Count;
        }
        public async Task<List<Order>> GetOrdersByDate(ReportDto reportDto)
        {
            List<Order> orders = await _firebaseServices.GetDataArray<Order,OrderItem>("CompletedOrders/"+ reportDto.BranchId);

            List<Order> eligibleOrders = FilterByDate(reportDto, orders); 

            //TODO: Create a guardClass so that we don't have to put too many try catch blocks
            // Checks if it recieved orders from the database, then to see if any of them fit the time period
            if (eligibleOrders.Count == 0 && orders != null) throw new IncorrectPeriodInputException($"The time period from {reportDto.StartDate} to {reportDto.EndDate} has no matching results");

            return eligibleOrders;
        }
        public async Task<List<Order>> GetAllOrdersByDate(ReportDto reportDto)
        {
            List<BranchDto> branches = new List<BranchDto>();

            var response = await _firebaseServices.GetData<Branch>("Branch");

            // Makes a branchDto for every Branch
            foreach (var item in response)
            {
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

            List<Order> orders = new List<Order>();
            // Gets orders for every branch
            for (int i = 0; i < branches.Count; i++)
            {
                orders=await _firebaseServices.GetDataArray<Order,OrderItem>(branches[i].Id);
            }

            return FilterByDate(reportDto, orders);

        }
        /// <summary>
        /// Filters whole order aggregates by date and coughs up the remainder orders that fit the condition
        /// <para> It is used only by <see cref="ReportServices"/></para>
        /// </summary>
        /// <param name="reportDto"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        private List<Order> FilterByDate(ReportDto reportDto, List<Order> orders)
        {
            List<Order> eligibleOrders = new List<Order>();
            foreach (var order in orders)
            {
                for (int i = 0; i < order.Count; i++)
                {
                    var orderItem = order[i];

                    DateTime orderTime = orderItem.OrderNumber.FromPixelProOrderNumbertoDateFormat();

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

        // Weight and Quantity
        public Order GetWeightByQuantity(Order items)
        {
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.Weight))
                {
                    if (item.Weight.Contains(' '))
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

        // Two month difference
        public async Task<(List<Order> ThisMonthorders, List<Order> LastMonthOrders)> GetTwoMonthOrders(ReportDto reportDto)
        {
            // Gets orders this month
            // If there is no branchId specified it will get all the orders of all the branches
            List<Order> orders = reportDto.BranchId==null?await GetAllOrdersByDate(reportDto): await GetOrdersByDate(reportDto);

            // Gets lasts months orders
            reportDto.StartDate = reportDto.StartDate.AddMonths(-1);
            reportDto.EndDate = reportDto.EndDate.AddMonths(-1);
            List<Order> lastMonthOrders = await GetOrdersByDate(reportDto);

            return (orders, lastMonthOrders);
        }
       
        // Filter methods
        public object OrderItemFilter<T>(string FilterType,  OrderItem item)
        {
            //UPDATE: It kept making null hashtable so this isn't helpful
            // So it doesn't throw null references, remove when we use enums
            //if (FilterType == null) return null;

            // REFACTOR: I tried using is but it refused so I just went with this
            if (typeof(T) == typeof(Hashtable))
            {
                Hashtable hashtable = new Hashtable();
                if (ExclusiveConditionFilter(FilterType, item.Name, item.Category))
                {
                    hashtable = GenerateOrderHashtable(item);
                }
                return hashtable;
            }
            if (typeof(T) == typeof(OrderItem))
            {
                if (ExclusiveConditionFilter(FilterType, item.Name, item.Category))
                {
                    return item;
                }
            }
            // If it didn't satisfy any of the other conditions 
            return null;

        }
        /// <summary>
        ///  This returns a bool based on Mutually exclusive conditions using the operator XOR (^)
        /// <para> Compares The <paramref name="FilterType"/>, eg category, to the <paramref name="typeName"/>.</para>
        /// <para> As it stands it checks if it does by both, XOR one of them XOR neither of them ( hence the true)</para>
        /// </summary>
        /// <param name="FilterType"></param>
        /// <param name="typeName"></param>
        /// <param name="typeCategory"></param>
        /// <returns></returns>
        private bool ExclusiveConditionFilter(string FilterType, string typeName, string typeCategory) => (FilterType == typeCategory && FilterType == typeName) ^
                    FilterType == typeCategory ^
                    FilterType == typeName ^
                    true;

        // Hashtable methods
        public ActionResult<Hashtable> GenerateReportHashtable(string metricname, float metric, float rawdifference, string rateOfchangePerDay)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add(metricname, metric);

            hashtable.Add("change", rateOfchangePerDay);

            hashtable.Add("positive", rawdifference < 0 ? false : true);

            return hashtable;
        }
        public List<Hashtable> GenerateSourcesHashtable(string[] sourceNames, int[] values)
        {
            List<Hashtable> hashtables = new List<Hashtable>();
            for (int i = 0; i < sourceNames.Length; i++)
            {
                hashtables[i].Add("name", sourceNames[i]);
                hashtables[i].Add("value", values[i]);
            }
            return hashtables;

        }
        /// <summary>
        /// Formats data into a form for the angular client to understand.
        /// <para> It is used only by the <see cref="ReportServices"/></para>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Hashtable GenerateOrderHashtable(OrderItem item)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("orderNumber", Format.OrderNumber(item.OrderNumber));
            hashtable.Add("itemName", item.Name);
            hashtable.Add("orderRevenue", Format.AmountToString(float.Parse(item.Price)));
            hashtable.Add("quantity", item.Quantity);

            if (!string.IsNullOrEmpty(item.Weight))
            {
                if (item.Weight.Contains(' '))
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


        
        public void InformBillToDeveloper()
        {
            // get word document from billingservices

            // turn it into a pdf

            // add pdf to an email

            // TODO: Send email to developers informing of sent bills.
            throw new NotImplementedException("InformBillToDeveloper() not done");
        }

        /// <summary>
        /// Sends the bill as an Email to the debtor
        /// </summary>
        public void SendBillToUser()
        {

            // get word document from billingservices

            // turn it into a pdf

            // add pdf to an email

            // TODO: Send email/ SMS 
            // TODO: Try merging work from the emailService branch since recent work was done in it

            throw new NotImplementedException("SendBillToUser() not done");

        }
        public INotification Notification { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    }
}
