import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-resetpassword',
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.css']
})
export class ResetpasswordComponent implements OnInit {

  model: any = {};

  constructor(private account: AccountService) { }

  ngOnInit(): void {
  }

  reset(){
    this.account.request(this.model, 'account/forgotpassword/' + this.model.accountID);
  }

}
