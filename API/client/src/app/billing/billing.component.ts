import { Component, OnInit } from '@angular/core';
import { retry } from 'rxjs/operators';
import { Branch } from '../_models/branch';
import { BillingService } from '../_services/billing.service';
import { ReferenceService } from '../_services/reference.service';

@Component({
  selector: 'app-billing',
  templateUrl: './billing.component.html',
  styleUrls: ['./billing.component.css']
})
export class BillingComponent implements OnInit {


  BilledBranches: Branch[];

  constructor(private billingService: BillingService, private referenceService: ReferenceService) { }

  ngOnInit(): void {
    this.GetBranches();
  }

  // Gets billed branches so that the view can make use of them
  GetBranches=()=>this.billingService.GetBilledBranches().subscribe(
    response=>{
      this.BilledBranches= response;
    }
    );
  // To give the view the total due for the user
  GetTotalPaymentDue=()=>this.billingService.GetTotalPaymentDue().subscribe(
    response=>{
      return response
    }
  );

}

