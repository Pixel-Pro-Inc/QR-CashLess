using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using RodizioSmartKernel.Entities;
using RodizioSmartKernel.Entities.Aggregates;
using RodizioSmartKernel.Interfaces.BaseInterfaces;

namespace API.Application.Interfaces
{
    public interface IReportServices: IBaseService
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
        public float FindRevenueinMonth(List<Order> orders);
        /// <summary>
        /// Give it a reportDto and it will cough up the revenue of the orders of this month and last month but it only uses the <see cref="IReportServices.GetAllOrdersByDate(ReportDto)"/>
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns>A <see cref="Tuple{T1, T2}"/> where <see cref="ValueTuple{T1}"/> is revenue this month and <see cref="ValueTuple{T2}"/> is from last month</returns>
        public Task<(float ThisMonthRevenue, float LastMonthRevenue)> GetTwoMonthsRevenue(ReportDto reportDto);
        /// <summary>
        /// This gets a List of Orders and finds the balance of which payment types each order was made with
        /// </summary>
        /// <param name="ordersgiven"></param>
        /// <param name="paymentTypes"></param>
        /// <param name="amountList"></param>
        /// <returns> A list of paymentdto's so the client can use them</returns>
        public List<PaymentDto> GetpaymentTypeBalances(List<Order> ordersgiven, string[] paymentTypes, float[] amountList);


        /// <summary>
        /// Give it a reportDto and it will cough up the orders of this month and last month 
        /// <para> If <see cref="ReportDto.BranchId"/> == null ( it wasn't set), it will get all the orders in all the branches</para>
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns>A <see cref="Tuple{T1, T2}"/> where <see cref="ValueTuple{T1}"/> is orders this month and <see cref="ValueTuple{T2}"/> is from last month</returns>
        public Task<(List<Order> ThisMonthorders, List<Order> LastMonthOrders)> GetTwoMonthOrders(ReportDto reportDto);
        /// <summary>
        /// Takes the gets all the orders from the <see cref="ReportDto.StartDate"/> and <see cref="ReportDto.EndDate"/> 
        /// <para> It also needs the <see cref="ReportDto"/> to have <see cref="ReportDto.BranchId"/> not null</para>
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns></returns>
        public Task<List<Order>> GetOrdersByDate(ReportDto reportDto);
        /// <summary>
        /// Takes the gets all the orders from the <see cref="ReportDto.StartDate"/> and <see cref="ReportDto.EndDate"/> 
        /// <para> It does this for all the branches and not just the ones provided</para>
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns></returns>
        public Task<List<Order>> GetAllOrdersByDate(ReportDto reportDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orders"></param>
        /// <returns>the number of days where the <paramref name="orders"/> where made</returns>
        public int GetNumberOfDaysElapsed(List<Order> orders);

        /// <summary>
        /// Mutiplies the weight of each item by the quality ordered  to get the Total weight
        /// </summary>
        /// <param name="items"></param>
        /// <returns>The orders with total weight</returns>
        public Order GetWeightByQuantity(Order items);
        // REFACTOR: We should change item Category and name to an enum for convience
        /// <summary>
        /// This checks the type of object it wants to return and filters the orderitem based on only the filtertype, including if there was no <paramref name="FilterType"/> set
        /// <para> This is, it will return an object if it matchs the conditions, else it will return null to the list</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="FilterType"></param>
        /// <param name="item"></param>
        /// <returns> Only single orderItems not whole order aggregates</returns>
        public object OrderItemFilter<T>(string FilterType, OrderItem item);

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
        /// <summary>
        /// Makes a hastable for each of the sources you put into it and another for the value of that source metric
        /// </summary>
        /// <param name="sourceNames"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public List<Hashtable> GenerateSourcesHashtable(string[] sourceNames, int[] values);

        /// <summary>
        /// Send email to developers about the ability to stop the branches functionality. Since the bill had already informed them it can terminate
        /// </summary>
        public void InformBillToDeveloper();
        /// <summary>
        /// Sends the bill as an Email to the debtor
        /// </summary>
        public void SendBillToUser();


    }
}
