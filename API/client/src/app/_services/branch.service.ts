import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { Branch } from '../_models/branch';
import { BaseServiceService } from './-base-service.service';
import { BusyService } from './busy.service';

@Injectable({
  providedIn: 'root'
})
export class BranchService extends BaseServiceService {

  constructor(http: HttpClient, private toastr: ToastrService, private busy: BusyService) {
    super(http);
  }

  submission(model: Branch, dir: string){
    this.busy.busy();
    return this.http.post(this.baseUrl + dir, model).subscribe(
      response =>{        
        this.toastr.success('Branch registered successfully');
        this.busy.idle();
        return response;
      },
      error =>
      {
        console.log(error);
        this.busy.idle();
      });
  }  

  getRestBranches(dir: string) {
    this.busy.busy();
    return this.http.get(this.baseUrl + dir).pipe(
      map((response: Branch[]) => {
        this.busy.idle();
        return response;
      })
    )
  }
}
