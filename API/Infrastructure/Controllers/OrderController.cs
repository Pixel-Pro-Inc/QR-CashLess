using API.Application.Extensions;
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

        [HttpPost("createorder/{branchId}")]
        public async Task<ActionResult<Order>> CreateOrder(Order orderItems, string branchId)
        {
            return (await new OrderFactory(branchId, _firebaseServices, notification).MakeOrder(orderItems, "Online")).AddPhoneNumber(orderItems[0].PhoneNumber);
        }

    }
}
