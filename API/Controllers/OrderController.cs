using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class OrderController : BaseApiController
    {
        private DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("getorders")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrder()
        {
            return await _context.OrderItems.ToListAsync() == null ? new List<OrderItem>() : await _context.OrderItems.ToListAsync();
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
