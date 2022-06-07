import { Component, OnInit } from '@angular/core';
import { retry } from 'rxjs/operators';
import { Branch } from '../_models/branch';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { BillingService } from '../_services/billing.service';

@Component({
  selector: 'app-billing',
  templateUrl: './billing.component.html',
  styleUrls: ['./billing.component.css']
})
export class BillingComponent implements OnInit {

  developer:boolean;
  BilledBranches: Branch[];
  adminUsers: User[];

  //This is so when you click the user it will flip to show them and the option to make them
  showUser:Boolean=false;

  constructor(private billingService: BillingService, private accountService: AccountService) { }

  ngOnInit(): void {
    this.developer=this.accountService.developer;
    //this.GetAdminUsers();
   // this.GetBranches();
  }
  

  /**
   *  
  Gets billed branches so that the view can make use of them
   *  */
  GetBranches=()=>this.BilledBranches= this.billingService.GetBilledBranches();

  /**
   * To give the view the total due for the user
   * @returns String
   */
  GetTotalPaymentDue=()=>this.billingService.GetTotalPaymentDue().subscribe(
    response=>{
      return response
    }
  );

  GetAdminUsers=()=>this.accountService.getAdminUsers().subscribe( response=>{this.adminUsers= response});


  //#region   View
    ChangeUserBillingState(model:User){
      this.billingService.setSelectedUserasBilledUser(model);
      this.FlipshowUserinView();
    }
  FlipshowUserinView=()=>!this.showUser;
  //#endregion

}

