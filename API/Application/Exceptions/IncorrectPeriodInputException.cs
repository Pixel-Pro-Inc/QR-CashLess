using System;
using System.Runtime.Serialization;

using API.DTOs;

namespace API.Exceptions
{
    /// <summary>
    /// We expect this to be thrown when the <see cref="ReportDto.StartDate"/> and <see cref="ReportDto.EndDate"/> can't be used.
    /// 
    /// <para> This might be the case if the date is before we started operating or a future date</para>
    /// </summary>
    [Serializable]
    internal class IncorrectPeriodInputException : Exception
    {
        public IncorrectPeriodInputException()
        {
        }

        public IncorrectPeriodInputException(string message) : base(message)
        {
        }

        public IncorrectPeriodInputException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IncorrectPeriodInputException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}