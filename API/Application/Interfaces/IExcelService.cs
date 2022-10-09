using Microsoft.AspNetCore.Mvc;
using RodizioSmartKernel.Core.Entities.Aggregates;
using RodizioSmartKernel.Application.Interfaces.BaseInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Application.Interfaces
{
    public interface IExcelService:IBaseService
    {
        /// <summary>
        /// This takses in the <paramref name="branchId"/>, and creates an excel based on all the orders in the specific branch
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns> It puts the excel file in the specified directory with everything it needs for the client</returns>
        public Task<FileStreamResult> ExportDataFromDatabase(string branchId);

        /// <summary>
        /// This is an overload of <see cref="ExportDataFromDatabase(string)"/> that takes in a specific list of orders and makes an excel file out of them 
        /// </summary>
        /// <param name="Orders"></param>
        /// <returns>It puts the excel file in the specified directory with everything it needs for the client</returns>
        public Task<FileStreamResult> ExportDataFromDatabase(List<Order> Orders);

    }
}
