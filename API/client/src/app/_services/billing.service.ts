import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { UnBillableUserException } from '../_exceptions/UnBillableUserException';
import { Billeduser } from '../_models/billeduser';
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
  CurrentUser: Billeduser;

  constructor(http: HttpClient,private branchService: BranchService,private accountService: AccountService) { 
    super(http);
  }

  /**
   * This is to return the billed branches from the current user
   * @return Branch[]
   * 
   * It does this by Getting all the billed users, 
   * then matching with the current user,
   * then getting all the branches ,
   * and returns the ones that match
   * 
   */
  GetBilledBranches(){
    
    //Checks and sets the CurrentUser if it is billable
    try {
      // FIXME: Remove this comment when youre done
      //this.setCurrentUserasBilledUser();
    } catch (exception) {
      if(exception instanceof UnBillableUserException) 
      {
        console.log(exception.name+" was thrown with this message: "+exception.message);
        // TODO: You were supposed to add the message that you arent billed yada yada, see exception descripiton for more details
        //throw new Error('Function not implemented. You were supposed to add the message that you arent billed yada yada');
      }

    }
    
    // what we will use to find the billed branches
   return this.filterBranches()

  }
  /**Gets the billed users from the database */
  GetBilledUsers(){
    return this.http.get(this.baseUrl + "billing/getbilledusers").pipe(
      map((response: Billeduser[]) => {
        return response;
      })
    );
  }
  GetTotalPaymentDue(){
    return this.http.get(this.baseUrl + "billing/paymentamount").pipe(
      map((response: string) => {
        return response;
      })
    );
  }
  /**
   * This method compares the billedUsers in the database to the current user 
   * and sets the current user to the billedUser equivalent.
   */
  setCurrentUserasBilledUser(){
    var users:Billeduser[];
     //gets all the billed users
     this.GetBilledUsers().subscribe(
      response=>{
        users=response;
      }
    );
    // This is to make sure we start on a fresh variable
    this.CurrentUser==null
    // sets the single user
    const currentUser= this.accountService.currentUser$.subscribe(
      response=>{
        users.forEach(_user => {
          // Checks if the current user matchs any of the billed users
          if(_user.username=response.username){
            this.CurrentUser= response as Billeduser;
            console.log(this.CurrentUser);
          } 
        })
        // If even after checking if the user is a BilledUser it comes up null it throws this expection
        if(this.CurrentUser==null)throw new UnBillableUserException(403,"You aren't a user that Pixel Pro bills");
      }
    );
  }
  /**
   * filteres collected branches to match what the user has
   */
  filterBranches():Branch[]{
    // UPDATE: so we put the filtering mechanism inside the filter branches
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
    
  
  


}


