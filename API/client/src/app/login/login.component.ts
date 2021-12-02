import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { ReferenceService } from '../_services/reference.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  model: any = {};
  user: User;

  constructor(public accountService: AccountService, private referenceService: ReferenceService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  login() {  
    
    console.log(this.model);

    this.accountService.login(this.model, 'account/login').subscribe(response => {
      this.accountService.currentUser$.subscribe(response => {
        this.user = response;
        console.log(this.user);
        if (this.user.admin) {
          this.router.navigateByUrl('/');
        }
        else {
          this.router.navigateByUrl('/');
        }
      });      
    }, error => {
      console.log(error);
      this.toastr.error(error.error);
    })
  }

}
