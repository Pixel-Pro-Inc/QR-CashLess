﻿using API.Application.Extensions;
using API.Application.Interfaces;
using API.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using RodizioSmartKernel.Core.Entities.Aggregates;
using RodizioSmartKernel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Infrastructure.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly string dir = "Order/";

        public OrderController(IFirebaseServices firebaseServices):base(firebaseServices) { }

        #region Create Order Item

        [HttpPost("createorder/{branchId}")]
        public async Task<ActionResult<Order>> CreateOrder(Order orderItems, string branchId)
        {
            return new OrderFactory(branchId,_firebaseServices,notification).AddPhoneNumber(orderItems[0].PhoneNumber);
        }
        public async Task<int> GetOrderNum(string branchId)
        {
            List<OrderNumber> ordersNumbersResult = await _firebaseServices.GetData<OrderNumber>("AssignedOrderNumbers/" + branchId);

            List<string> oNumbersAssigned = new List<string>();

            if (ordersNumbersResult.Count > 0)
                oNumbersAssigned = ordersNumbersResult[0].OrderNumbers;

            string d = DateTime.Now.Day.ToString("00") + "-" + DateTime.Now.Month.ToString("00") + "-"
                    + DateTime.Now.Year.ToString("0000");

            bool clearList = false;

            foreach (var oNumber in oNumbersAssigned)
            {
                if(oNumber.Substring(0, 10) != d)
                {
                    clearList = true;
                }
            }

            if(clearList)
                oNumbersAssigned.Clear();

            int candidateNumber = new Random().Next(1000, 9999);

            //Check Against Other Order Numbers For The Day
            while (oNumbersAssigned.Contains(d + "_" + candidateNumber))
            {
                candidateNumber = new Random().Next(1000, 9999);
            }

            oNumbersAssigned.Add(d + "_" + candidateNumber);

            _firebaseServices.StoreData("AssignedOrderNumbers/" + branchId + "/0", new OrderNumber() { OrderNumbers = oNumbersAssigned});

            return candidateNumber;
        }
        #endregion
    }
}
