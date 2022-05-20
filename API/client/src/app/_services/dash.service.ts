import { HttpClient, HttpEventType, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { ReportItem } from '../_models/report';
import { BaseServiceService } from './-base-service.service';

@Injectable({
  providedIn: 'root'
})
export class DashService extends BaseServiceService{

  constructor(http: HttpClient, private toastr: ToastrService) {
    super(http);
  }

  totalSales(model: any){
    return this.http.post(this.baseUrl + 'report/sales/total', model).pipe(
      map((response: ReportItem[]) => {
        return response;
      })
    )
  }

  totalDetailedSales(model: any){
    return this.http.post(this.baseUrl + 'report/sales/item', model).pipe(
      map((response: ReportItem[]) => {
        return response;
      })
    )
  }

  revenue(model: any){
    return this.http.post(this.baseUrl + 'report/sales/summary', model).pipe(
      map((response: ReportItem) => {
        return response;
      })
    )
  }

  payment(model: any){
    return this.http.post(this.baseUrl + 'report/sales/paymentmethods', model).pipe(
      map((response: PaymentItem[]) => {
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
  invoice(model: any){
    return this.http.post(this.baseUrl + 'report/sales/invoice', model).pipe(
      map((response: any) => {
        return response;
      })
    )
  }

  exportToExcel(branchId: string){
    window.open(this.baseUrl + 'excel/export/' + branchId);
  }
  detailReportToExcel(model: any)
  {
    return this.http.post(this.baseUrl + 'report/excel/export', model).pipe(
      map((response: ReportItem[]) => {
        return response;
      })
    )
  }

}