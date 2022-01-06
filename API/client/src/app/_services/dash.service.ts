import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { ReportItem } from '../_models/report';

@Injectable({
  providedIn: 'root'
})
export class DashService {

  baseUrl = 'https://localhost:5001/api/';
  //baseUrl = 'https://rodizioexpress.azurewebsites.net/api/';

  constructor(private http: HttpClient, private toastr: ToastrService) { }

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
}
