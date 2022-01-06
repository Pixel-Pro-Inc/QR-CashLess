import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { Branch } from '../_models/branch';

@Injectable({
  providedIn: 'root'
})
export class BranchService {
  //baseUrl = 'https://localhost:5001/api/';
  baseUrl = 'https://rodizioexpress.azurewebsites.net/api/';

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  submission(model: Branch, dir: string){
    return this.http.post(this.baseUrl + dir, model).subscribe(
      response =>{
        this.toastr.success('Branch registered successfully');
        return response;
      },
      error =>
      {
        console.log(error);
      });
  }  

  getRestBranches(dir: string) {
    return this.http.get(this.baseUrl + dir).pipe(
      map((response: Branch[]) => {
        return response;
      })
    )
  }
}
