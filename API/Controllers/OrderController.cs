using API.Data;
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
            private readonly FirebaseDataContext _firebaseDataContext;
            private readonly string dir = "Order/";

        public OrderController()
        {
            _firebaseDataContext = new FirebaseDataContext();
        }

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

                orderItem.Price = orderItem.Price;

                string z = orderItem.PhoneNumber;

                orderItem.Id = i;

                _firebaseDataContext.StoreData(dir + branchId + "/" + orderItem.OrderNumber + "/" + orderItem.Id, orderItem);
            }

            return orderItems;
        }
        public async Task<int> GetOrderNum(string branchId)
        {
            var response = await _firebaseDataContext.GetData(dir + branchId);
            List<OrderItem> orders = new List<OrderItem>();

            foreach (var item in response)
            {
                OrderItem[] data = JsonConvert.DeserializeObject<OrderItem[]>(((JArray)item).ToString());

                for (int i = 0; i < data.Length; i++)
                {
                    orders.Add(data[i]);
                }                
            }

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

        /*
        [HttpGet("getorders")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrder()
        {
            return await _context.OrderItems.ToListAsync() == null ? new List<OrderItem>() : await _context.OrderItems.ToListAsync();
        }        
        [HttpPost("editorder")]
        public async Task<ActionResult<OrderItem>> EditOrder(OrderItem orderItem)
        {
            _context.Entry(orderItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return orderItem;
        }
        */
    }
}
