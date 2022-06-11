using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IReportServices
    {
        /// <summary>
        /// Finds the total sales made in the time period given by the <see cref="ReportDto"/> 
        /// <para> 
        /// It use the <see cref="ReportDto.StartDate"/> and <see cref="ReportDto.EndDate"/> NOTE: It needs the branchid as well
        /// </para>
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns>Sales as <see cref="float"/></returns>
        public Task<float> GetSalesAmountinTimePeriod(ReportDto reportDto);

        /// <summary>
        /// Takes the gets all the orders from the <see cref="ReportDto.StartDate"/> and <see cref="ReportDto.EndDate"/> 
        /// <para> It doesn't need any other properties of the <see cref="ReportDto"/></para>
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns></returns>
        public Task<List<List<OrderItem>>> GetOrdersByDate(ReportDto reportDto);

    }
}
