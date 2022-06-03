﻿using API.Data;
using API.Entities;
using API.Interfaces;
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

        public OrderController(IFirebaseServices firebaseServices):base(firebaseServices) { }

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

                _firebaseServices.StoreData(dir + branchId + "/" + orderItem.OrderNumber + "/" + orderItem.Id, orderItem);
            }

            return orderItems;
        }
        public async Task<int> GetOrderNum(string branchId)
        {
            List<OrderItem> orders = await _firebaseServices.GetOrders(branchId);

            int candidateNumber = new Random().Next(1000, 9999);

            //Check Against Other Order Numbers For The Day
            List<int> orderNums = new List<int>();

            for (int i = 0; i < orders.Count; i++)
            {
                //Only 4 digit numbers

                string orderNum = orders[i].OrderNumber;

                int n = orderNum.IndexOf('_');

                string date = orderNum.Remove(n, 5);

                string number = orderNum.Remove(0, n + 1);

                string dateToday =
                    DateTime.Now.Day.ToString("00") + "-"
                    + DateTime.Now.Month.ToString("00") + "-"
                    + DateTime.Now.Year.ToString("0000");

                if (date == dateToday)
                {
                    orderNums.Add((Int32.Parse(number)));
                }
            }

            while (orderNums.Contains(candidateNumber))
            {
                candidateNumber = new Random().Next(1000, 9999);
            }

            return candidateNumber;
        }
        #endregion
    }
}
