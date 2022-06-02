import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-accountmanagement',
  templateUrl: './accountmanagement.component.html',
  styleUrls: ['./accountmanagement.component.css']
})
export class AccountmanagementComponent implements OnInit {

  developer:boolean=true;// TODO: have this get brough in from the nav component
  constructor() { }

  ngOnInit(): void {
  }

  fil(){}
}
