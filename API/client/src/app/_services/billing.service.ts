import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { Billeduser } from '../_models/billeduser';
import { Branch } from '../_models/branch';
import { User } from '../_models/user';
import { BaseServiceService } from './-base-service.service';

@Injectable({
  providedIn: 'root'
})
export class BillingService extends BaseServiceService {
  branches: Branch[];
  users: User[];

  constructor(http: HttpClient) { 
    super(http);
  }


  GetBilledBranches(){
    // TODO: Have this get the billedUsers in the API and feed it the branches it manages
     return this.http.get(this.baseUrl + "Give it something").pipe(
      map((response: Branch[]) => {
        this.branches= response;
        return response;
      })
    );
  }
  GetBilledUsers():Billeduser[]{
    // TODO: Have this get the billedUsers 
    throw new Error('Function not implemented.');
    //return this.users;
  }
  GetTotalPaymentDue(){
    return this.http.get(this.baseUrl + "billing/paymentamount").pipe(
      map((response: string) => {
        return response;
      })
    );
  }

}

