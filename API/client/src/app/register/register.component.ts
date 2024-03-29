import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { Branch } from '../_models/branch';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { BranchService } from '../_services/branch.service';
import { ReferenceService } from '../_services/reference.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  branches: Branch[] = [];
  model: any = {};
  user: User;

  isDeveloper = false;

  constructor(public accountService: AccountService, private toastr: ToastrService, private branchService: BranchService, private referenceService: ReferenceService) { }

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe(
      r => {
        this.user = r;

        this.isDeveloper = this.user.developer;
      }
    );

    this.branchService.getRestBranches('branch/getbranches').subscribe(response => {
      this.branches = response;
    },
    error => {
      console.log(error);
    });
  }

  register() {
    console.log(this.model);
    
    if(this.model.branchId == null){
      let branch: string[] = [this.referenceService.currentBranch()];
      this.model.branchId = branch;
    }

    if(this.model.superUser){
      this.resetBranches();
    }
    
    this.accountService.register(this.model, 'account/register').subscribe(response => {
      console.log(response);
      this.toastr.success("Your account was registered successfully");
      this.cancel();
    }, error => {
        console.log(error);
        this.toastr.error(error.error);
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
  
  resetBranches(){
    this.model.branchId = null;
  }

}
