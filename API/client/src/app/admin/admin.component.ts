import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ReceiptComponent } from '../receipt/receipt.component';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { AdminRightService } from '../_services/admin-right.service';
import { BranchesService } from '../_services/branches.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  model: any = {};
  user: User;

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService, private branchService: BranchesService, private adminPower:AdminRightService) { }

  ngOnInit(): void {
  }

  login() {    

    this.accountService.login(this.model, 'account/login').subscribe(response => {
      this.accountService.currentUser$.subscribe(response => {
        this.user = response;
        console.log(this.user);
        if (this.user.admin) {
          this.router.navigateByUrl('/menu/edit');
        }
        else {
          this.router.navigateByUrl('/kitchen');
        }
      });      
    }, error => {
      console.log(error);
      this.toastr.error(error.error);
    })
  }



  addRestBranch() {
    this.adminPower.addBranch(this.model, 'branch/addBranch').subscribe(response => {
      console.log(response); //I dont know if this will work cause I never made a branch page
    }
    );
  }
  deleteRestBranch() {
    this.adminPower.deleteBranch(this.model, 'branch/delete').subscribe(response => {
      console.log(response); //I dont know if this will work cause I never made a branch page
    }
    );
  }
  checkRestBranch(model:any) {
    if (model == typeof (String)) {
      this.adminPower.checkBranch(model);
    } else {
      console.log('wrong type');
    }
     
  }

  //don't touch this, its used by another component
  pushreceipt(model: any ) {
    if (model == typeof (ReceiptComponent)) {
      this.adminPower.pushtoReceiptDic(model);
    } else {
      console.log('wrong type');
    }
      
  }
  getreceipt(slipNumber:number) {
    this.adminPower.getreceipt(slipNumber);
  }

  pushReceiptList() {
    this.adminPower.pushtoReceiptBucket('receipt/pushreceipts', this.model).subscribe(response => {
      console.log(response);
      window.location.reload();
    });
  }
  pullReceiptList() {
    this.adminPower.getreceiptList('receipt/pullreceipts', this.model).subscribe(response => {
      console.log(response);
      window.location.reload();
    })
  }
}
