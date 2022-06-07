using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Exceptions
{
    /// <summary>
    /// This is thrown when a <see cref="AppUser"/> is found not to be a <see cref="AdminUser"/> type.
    /// 
    /// We can only bill people who are to be billed, not employees and the like, so a new class was created to inherit from
    /// <see cref="AppUser"/> and hopefully we can have all Billed Users reregistered under the <see cref="AdminUser"/> tag
    /// </summary>
    [Serializable]
    [Obsolete]
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
