using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

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
        /// Takes in any collection of <paramref name="orders"/> and finds the revenue 
        /// </summary>
        /// <param name="orders"></param>
        /// <returns> revenue as a <see cref="float"/></returns>
        public float FindRevenueinMonth(List<List<OrderItem>> orders);


        /// <summary>
        /// Give it a reportDto and it will cough up the orders of this month and last month
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns>A <see cref="Tuple{T1, T2}"/> where <see cref="ValueTuple{T1}"/> is orders this month and <see cref="ValueTuple{T2}"/> is from last month</returns>
        public Task<(List<List<OrderItem>> ThisMonthorders, List<List<OrderItem>> LastMonthOrders)> GetTwoMonthOrders(ReportDto reportDto);
        /// <summary>
        /// Takes the gets all the orders from the <see cref="ReportDto.StartDate"/> and <see cref="ReportDto.EndDate"/> 
        /// <para> It doesn't need any other properties of the <see cref="ReportDto"/></para>
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns></returns>
        public Task<List<List<OrderItem>>> GetOrdersByDate(ReportDto reportDto);

        /// <summary>
        /// Returns If the <see cref="ReportDto"/> coming in is required to be filtered by Name. 
        /// <para>
        /// Does this by checking if the property is null</para>
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns></returns>
        public bool FilterByCategory(ReportDto reportDto);
        /// <summary>
        /// Returns If the <see cref="ReportDto"/> coming in is required to be filtered by Catergory. 
        /// <para>
        /// Does this by checking if the property is null</para>
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns></returns>
        public bool FilterByName(ReportDto reportDto);

        /// <summary>
        /// This create a <see cref="Hashtable"/> for the view in the client of the report metric you desire. You have to provide:
        /// <para> <paramref name="metricname"/> Which is the metric you are trying to display</para>
        /// <para> <paramref name="metric"/> Which is the actual value you got that month,</para>
        /// <para> <paramref name="rawdifference"/> in value between the two months,</para>
        /// <para> <paramref name="rateOfchangePerDay"/> which is an average of the raw per day of the metric</para>
        /// 
        /// </summary>
        /// <param name="metricname"></param>
        /// <param name="metric"></param>
        /// <param name="rawdifference"></param>
        /// <param name="rateOfchangePerDay"></param>
        /// <returns></returns>
        public ActionResult<Hashtable> GenerateReportHashtable(string metricname, float metric, float rawdifference, string rateOfchangePerDay);

    }
}
