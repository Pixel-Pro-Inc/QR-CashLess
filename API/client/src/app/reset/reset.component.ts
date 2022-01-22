import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.css']
})
export class ResetComponent implements OnInit {

  model: any = {};

  constructor(private account: AccountService) { }

  ngOnInit(): void {
  }

  reset(){
    this.account.reset(this.model, 'account/forgotpassword/successful/' + localStorage.getItem('accountID') + '/' + this.model.password);
  }

  getToken(){
    return localStorage.getItem('resetToken');
  }

}
