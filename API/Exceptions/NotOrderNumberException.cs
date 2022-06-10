using System;
using System.Runtime.Serialization;

namespace API.Extensions
{
    /// <summary>
    ///  This should be thrown when the string you give isn't in the correct OrderNumber format or is not at all even and OrderNumber
    ///  <para> Check <see cref="DateTimeFormatExtensions.FromPixelProOrderNumbertoDateFormat(string)"/> for reference on the format</para>
    /// </summary>
    [Serializable]
    internal class NotOrderNumberException : Exception
    {
        public NotOrderNumberException()
        {
        }

        public NotOrderNumberException(string message) : base(message)
        {
        }

        public NotOrderNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotOrderNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}