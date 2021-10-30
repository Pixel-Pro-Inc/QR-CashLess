import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { BranchesService } from '../_services/branches.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  model: any = {};
  user: User;

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService, private branchService: BranchesService) { }

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
    this.branchService.addRestBranch(this.model, 'branch/addBranch').subscribe(response => {
      console.log(response); //I dont know if this will work cause I never made a branch page
    }
    );
  }
}
