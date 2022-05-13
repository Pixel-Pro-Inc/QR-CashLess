using System;
using System.Runtime.Serialization;

namespace API.Controllers
{
    /// <summary>
    /// This is to be thrown if you don't give the ExcelController.CreateExportToExcelDto() the expected object.
    /// This object is taken from the dashservice after it arrives from the ReportController
    /// </summary>
    [Serializable]
    public class WrongTypeToMakeExcelDto : Exception
    {
        public WrongTypeToMakeExcelDto() { }
        public WrongTypeToMakeExcelDto(string message) : base(message) { }
        public WrongTypeToMakeExcelDto(string message, Exception inner) : base(message, inner) { }
        protected WrongTypeToMakeExcelDto(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }
}