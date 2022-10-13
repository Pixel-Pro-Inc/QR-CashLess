﻿using System;
using RodizioSmartKernel.Core.Entities;
using API.Infrastructure.Services;

namespace API.Application.Exceptions
{
    /// <summary>
    /// This is thrown when a <see cref="AppUser"/> is found not to be a <see cref="AdminUser"/> type.
    /// 
    /// We can only bill people who are to be billed, not employees and the like, so a new class was created to inherit from
    /// <see cref="AppUser"/> 
    /// <para>
    /// NOTE: That all admin Users billable, which means this is most likely to be thrown in <see cref="BillingServices.SetUser(AppUser)"/> 
    /// </para>
    /// </summary>
    [Serializable]
    public class UnBillableUserException : Exception
    {
        public UnBillableUserException() { }
        public UnBillableUserException(string message) : base(message) { }
        public UnBillableUserException(string message, Exception inner) : base(message, inner) { }
        protected UnBillableUserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}