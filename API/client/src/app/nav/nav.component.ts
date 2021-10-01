import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { error } from 'selenium-webdriver';
import { User } from '../models/User';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {} /** This is a class property to store what the user inputs into the form **/

  constructor(public accountservice:AccountService) { }

  ngOnInit(): void {
  }

  login() {
    this.accountservice.login(this.model).subscribe(response => {
      console.log(response)
    }, error => {
      console.log(error);
    })
  }
  logout() {
    this.accountservice.logout();
  }
}
