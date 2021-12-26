import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { saveAs } from 'file-saver';

@Injectable({
  providedIn: 'root'
})
export class DashService {

  baseUrl = 'https://localhost:5001/api/';
  //baseUrl = 'https://rodizioexpress.azurewebsites.net/api/';

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  totalSales(model: any){
    this.http.get(this.baseUrl + 'reports/sales/total', model).subscribe(
      response =>{
        return response;
      },
      error =>{
        this.toastr.error(error.error);
      }
    );
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
