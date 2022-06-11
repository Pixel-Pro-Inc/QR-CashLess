import { HttpClient, HttpEventType, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { ReportItem } from '../_models/report';
import { BaseServiceService } from './-base-service.service';
import { BusyService } from './busy.service';

@Injectable({
  providedIn: 'root'
})
export class DashService extends BaseServiceService{

  constructor(http: HttpClient, private toastr: ToastrService, private busyService: BusyService) {
    super(http);
  }
  // REFACTOR: The below three methods could be refactored. We can't have repeating code for such important business logic
  totalSales(model: any){
    this.busyService.busy_1();
    return this.http.post(this.baseUrl + 'report/sales/total', model).pipe(
      map((response: ReportItem[]) => {
        this.busyService.idle_1();
        return response;
      })
    )
  }

  totalDetailedSales(model: any){
    this.busyService.busy_1();
    return this.http.post(this.baseUrl + 'report/sales/item', model).pipe(
      map((response: ReportItem[]) => {
        this.busyService.idle_1();
        return response;
      })
    )
  }

  revenue(model: any){
    this.busyService.busy_1();
    return this.http.post(this.baseUrl + 'report/sales/summary', model).pipe(
      map((response: ReportItem) => {
        this.busyService.idle_1();
        return response;
      })
    )
  }

  payment(model: any){
    this.busyService.busy_1();
    return this.http.post(this.baseUrl + 'report/sales/paymentmethods', model).pipe(
      map((response: PaymentItem[]) => {
        this.busyService.idle_1();
        return response;
      })
    )
  }

  salesVolume(branchId: string){
    return this.http.get(this.baseUrl + 'report/sales/thismonth/volume/' + branchId).pipe(
      map((response: any) => {
        return response;
      })
    )
  }
  salesRevenue(branchId: string){
    return this.http.get(this.baseUrl + 'report/sales/thismonth/revenue/' + branchId).pipe(
      map((response: any) => {
        return response;
      })
    )
  }
  allSalesRevenue(){
    return this.http.get(this.baseUrl + 'report/sales/thismonth/allrevenue').pipe(
      map((response: any) => {
        return response;
      })
    )
  }
  salesAverage(branchId: string){
    return this.http.get(this.baseUrl + 'report/sales/thismonth/averagevolume/' + branchId).pipe(
      map((response: any) => {
        return response;
      })
    )
  }
  revenueAverage(branchId: string){
    return this.http.get(this.baseUrl + 'report/sales/thismonth/averagerevenue/' + branchId).pipe(
      map((response: any) => {
        return response;
      })
    )
  }
  itemsAverage(branchId: string){
    return this.http.get(this.baseUrl + 'report/sales/thismonth/averageitems/' + branchId).pipe(
      map((response: any) => {
        return response;
      })
    )
  }
  orderSource(branchId: string){
    return this.http.get(this.baseUrl + 'report/sales/thismonth/ordersource/' + branchId).pipe(
      map((response: any) => {
        return response;
      })
    )
  }
  invoice(model: any){
    this.busyService.busy_1();
    return this.http.post(this.baseUrl + 'report/sales/invoice', model).pipe(
      map((response: any) => {
        this.busyService.idle_1();
        return response;
      })
    )
  }

  exportToExcel(branchId: string){
    window.open(this.baseUrl + 'excel/export/' + branchId);
  }

  exportDetailReportToExcel(model: any){
    this.http.post(this.baseUrl + 'report/excel/export-detailedsales', model,{responseType: 'blob' as 'json'}).subscribe(
      (response: any) =>{
          let dataType = response.type;
          let binaryData = [];
          binaryData.push(response);
          let downloadLink = document.createElement('a');
          downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, {type: dataType}));
          downloadLink.setAttribute('download', 'Rodizio Express Data_Export ' + (new Date()).toString());
          document.body.appendChild(downloadLink);
          downloadLink.click();
      }
  );
  }

  exportTotalReportToExcel(model: any){
    this.http.post(this.baseUrl + 'report/excel/export-totalsales', model,{responseType: 'blob' as 'json'}).subscribe(
      (response: any) =>{
          let dataType = response.type;
          let binaryData = [];
          binaryData.push(response);
          let downloadLink = document.createElement('a');
          downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, {type: dataType}));
          downloadLink.setAttribute('download', 'Rodizio Express Data_Export ' + (new Date()).toString());
          document.body.appendChild(downloadLink);
          downloadLink.click();
      }
  );
  }
}
