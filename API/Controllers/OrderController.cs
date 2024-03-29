﻿using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly string dir = "Order/";

        public OrderController():base() { }

        #region Create Order Item

        [HttpPost("createorder/{branchId}")]
        public async Task<ActionResult<List<OrderItem>>> CreateOrder(List<OrderItem> orderItems, string branchId)
        {
            int x = await GetOrderNum(branchId);

            string d = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/"
                    + DateTime.Now.Year.ToString("0000");

            for (int i = 0; i < orderItems.Count; i++)
            {
                var orderItem = orderItems[i];

                orderItem.OrderNumber = d + "_" + x;
                orderItem.OrderNumber = orderItem.OrderNumber.Replace('/', '-');

                orderItem.OrderDateTime = DateTime.UtcNow;

                orderItem.Price = orderItem.Price;

                string z = orderItem.PhoneNumber;

                orderItem.Id = i;

                _firebaseDataContext.StoreData(dir + branchId + "/" + orderItem.OrderNumber + "/" + orderItem.Id, orderItem);
            }

            return orderItems;
        }
        public async Task<int> GetOrderNum(string branchId)
        {
            List<OrderNumber> ordersNumbersResult = await _firebaseDataContext.GetData<OrderNumber>("AssignedOrderNumbers/" + branchId);

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

            _firebaseDataContext.StoreData("AssignedOrderNumbers/" + branchId + "/0", new OrderNumber() { OrderNumbers = oNumbersAssigned});

            return candidateNumber;
        }
        #endregion
    }
}
