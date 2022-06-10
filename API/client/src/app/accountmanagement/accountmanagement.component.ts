import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-accountmanagement',
  templateUrl: './accountmanagement.component.html',
  styleUrls: ['./accountmanagement.component.css']
})
export class AccountmanagementComponent implements OnInit {

  developer:boolean=this.accountService.developer
  
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  fil(){}
}
