using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using System;
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

        public async Task<float> GetSalesAmountinTimePeriod(ReportDto reportDto)
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

            return SalesByItem.SummaryTotal;
        }
        public async Task<List<List<OrderItem>>> GetOrdersByDate(ReportDto reportDto)
        {
            List<List<OrderItem>> eligibleOrders = new List<List<OrderItem>>();

            List<List<OrderItem>> orders = await _firebaseServices.GetOrders("CompletedOrders/", reportDto.BranchId);

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

            //TODO: Create a guardClass so that we don't have to put too many try catch blocks
            // Checks if it recieved orders from the database, then to see if any of them fit the time period
            if (eligibleOrders.Count == 0&&orders!=null) throw new IncorrectPeriodInputException($"The time period from {reportDto.StartDate} to {reportDto.EndDate} has no matching results");

            return eligibleOrders;
        }

    }
}
