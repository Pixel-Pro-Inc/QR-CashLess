import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

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
}
