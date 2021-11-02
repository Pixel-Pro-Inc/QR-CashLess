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
        private DataContext _context;
        private readonly FirebaseDataContext _firebaseDataContext;

        public OrderController(DataContext context)
        {
            _context = context;
            _firebaseDataContext = new FirebaseDataContext();
        }

        [HttpGet("getorders")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrder()
        {
            return await _context.OrderItems.ToListAsync() == null ? new List<OrderItem>() : await _context.OrderItems.ToListAsync();
        }
        [HttpGet("getnumber/{branchId}")]
        public async Task<ActionResult<int>> GetOrderNum(string branchId)
        {
            var response = await _firebaseDataContext.GetData("Order/" + branchId);
            List<OrderItem> orders = new List<OrderItem>();

            foreach (var item in response)
            {
                OrderItem data = JsonConvert.DeserializeObject<OrderItem>(((JObject)item).ToString());

                orders.Add(data);
            }

            for (int i = 0; i < orders.Count; i++)
            {
                string date = ;
                string number = ;

                string dateToday = 
                    DateTime.Now.Day.ToString("00") + "/" 
                    + DateTime.Now.Month.ToString("00") + "/" 
                    + DateTime.Now.Year.ToString("00");

                if(date == dateToday)
            }
        }
        [HttpPost("createorder")]
        public async Task<ActionResult<OrderItem>> CreateOrder(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            return orderItem;
        }
        [HttpPost("editorder")]
        public async Task<ActionResult<OrderItem>> EditOrder(OrderItem orderItem)
        {
            _context.Entry(orderItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return orderItem;
        }
    }
}
