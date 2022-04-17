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
    this.busyService.busy();
    return this.http.post(this.baseUrl + 'report/sales/total', model).pipe(
      map((response: ReportItem[]) => {
        this.busyService.idle();
        return response;
      })
    )
  }

  totalDetailedSales(model: any){
    this.busyService.busy();
    return this.http.post(this.baseUrl + 'report/sales/item', model).pipe(
      map((response: ReportItem[]) => {
        this.busyService.idle();
        return response;
      })
    )
  }

  revenue(model: any){
    this.busyService.busy();
    return this.http.post(this.baseUrl + 'report/sales/summary', model).pipe(
      map((response: ReportItem) => {
        this.busyService.idle();
        return response;
      })
    )
  }

  payment(model: any){
    this.busyService.busy();
    return this.http.post(this.baseUrl + 'report/sales/paymentmethods', model).pipe(
      map((response: PaymentItem[]) => {
        this.busyService.idle();
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
    this.busyService.busy();
    return this.http.post(this.baseUrl + 'report/sales/invoice', model).pipe(
      map((response: any) => {
        this.busyService.idle();
        return response;
      })
    )
  }

  exportToExcel(branchId: string){
    window.open(this.baseUrl + 'excel/export/' + branchId);
  }
}
