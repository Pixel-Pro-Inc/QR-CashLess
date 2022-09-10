import { error } from '@angular/compiler/src/util';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { async, Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { ReferenceService } from '../_services/reference.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  admin = false;
  developer = false;
  kitchenStaff = false;
  user: User;

  constructor(public referenceService: ReferenceService, public accountService: AccountService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe(
      response => {
        this.user = response;
        
        if (this.user != null)
        {
          if (this.user.admin) {
            this.admin = true;
          }
          else if(this.user.developer) {
            // This is so when ever we need to access the developer boolean it will be set
            this.accountService.developer=this.developer = true;  
          }
          else{
            this.kitchenStaff = true;
          }
        }
      });    
  } 

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/login');
  }
}
