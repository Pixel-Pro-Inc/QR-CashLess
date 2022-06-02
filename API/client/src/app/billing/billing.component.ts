import { Component, OnInit } from '@angular/core';
import { retry } from 'rxjs/operators';
import { Branch } from '../_models/branch';
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

  constructor(private billingService: BillingService, private accountService: AccountService) { }

  ngOnInit(): void {
    this.developer=this.accountService.developer;
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

}

