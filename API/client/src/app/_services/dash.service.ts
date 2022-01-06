import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { ReportItem } from '../_models/report';
import { saveAs } from 'file-saver';
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

  importexceldata(model: any, filename:any) {
    this.http.post(this.baseUrl + 'excel/importexceldata/' + filename, model).subscribe(response => {
      localStorage.setItem('order', JSON.stringify(response));
      console.log(response);
    },
      error => {
        console.log(error);
      });
  }

  exportexceldata() {
    //It throws a blob error. I honestly give up. All i want is for it to prompt a download window.
    this.http.get(this.baseUrl + 'excel/exportexceldata', { responseType: "blob", headers: { 'Accept': 'application/xlsx' } })
      .subscribe(blob => {
        saveAs(blob, 'download.xlsx');
      },//Im not sure this code works but i copied it from this link https://stackoverflow.com/questions/35138424/how-do-i-download-a-file-with-angular2-or-greater
        error => {
          this.toastr.error(error.error);
        });
    //I tried importing the SaveDialog class but it did not work cause its used in windows form




    /**
     * this.http.get(this.baseUrl + 'excel/exportexceldata', path).subscribe(
      response => {
        return response;
      },
      error => {
        this.toastr.error(error.error);
      }
    );
    **/
    
  }

}
