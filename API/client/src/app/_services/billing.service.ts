import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { UnBillableUserException } from '../_exceptions/UnBillableUserException';
import { AdminUser } from '../_models/billeduser';
import { Branch } from '../_models/branch';
import { User } from '../_models/user';
import { BaseServiceService } from './-base-service.service';
import { AccountService } from './account.service';
import { BranchService } from './branch.service';

@Injectable({
  providedIn: 'root'
})
export class BillingService extends BaseServiceService {
  branches: Branch[];
  CurrentUser: AdminUser;

  constructor(http: HttpClient,private branchService: BranchService,private accountService: AccountService) { 
    super(http);
  }

  /**
   * This is to return the billed branches from the current user
   * @return Branch[]
   * 
   * It does this by getting all the branches ,
   * and returns the ones that match with the ones that the users list contains
   * 
   */
  GetBilledBranches(){
    // UPDATE: We removed the selectedUser method cause admin come in as default    
     // UPDATE: so we put the filtering mechanism inside the filter branches

     // filteres collected branches to match what the user manages
  this.branchService.getRestBranches('branch/getbranches').subscribe(response => {
    this.branches=response;
    console.log(response+"so it got the branches");
    }, error=>{
    console.log(error);
    });
    // FIXME: Test to see if this actually works the way it is intended

    // gets the branches it manages by selecting results where the predicate ==true
    this.branches.filter(branch => this.CurrentUser.BilledBranchIds.find(b=> b===branch.id));
    console.log(this.branches+"Now it filtered them");

    return this.branches;

  }
  
  GetTotalPaymentDue(){
    return this.http.get(this.baseUrl + "billing/paymentamount").pipe(
      map((response: string) => {
        return response;
      })
    );
  }
  // UPDATE: Since admin by default there is no need to be able to switch users to and fro billed users
 

}


