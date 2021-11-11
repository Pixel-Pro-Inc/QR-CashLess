import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ReceiptComponent } from '../receipt/receipt.component';
import { AccountService } from '../_services/account.service';
import { AdminRightService } from '../_services/admin-right.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService, private adminPower: AdminRightService) { }
  model: any = {}
  ngOnInit(): void {
  }

  pushreceipt(model: any) {
    if (model == typeof (ReceiptComponent)) {
      //this.adminPower.pushtoReceiptDic(model);
    } else {
      console.log('wrong type');
    }

  }
  getreceipt(slipNumber: number) {
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
