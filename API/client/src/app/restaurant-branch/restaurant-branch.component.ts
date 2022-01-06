import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { userInfo } from 'os';
import { Branch } from '../_models/branch';
import { OrderItem } from '../_models/orderItem';
import { User } from '../_models/user';
import { BranchService } from '../_services/branch.service';
import { ReferenceService } from '../_services/reference.service';

@Component({
  selector: 'app-restaurant-branch',
  templateUrl: './restaurant-branch.component.html',
  styleUrls: ['./restaurant-branch.component.css']
})
export class RestaurantBranchComponent implements OnInit {
  constructor(private branchService: BranchService, private router: Router, private referenceService: ReferenceService) { }
  RestBranches: Branch[] = [];

  title = 'Pick a restaurant branch most convenient to you';

  ngOnInit(): void {
    let user: User = JSON.parse(localStorage.getItem('user'));

    let empty: OrderItem[] = [];
    localStorage.setItem('ordered', JSON.stringify(empty));

    if(user == null){
      this.getBranches();
      return;
    }

    this.title = 'Choose a branch to manage';

    if(user.superUser){
      this.getBranches();
      return;
    }
    
    if(user.developer){
      this.getBranches();
      return;
    }

    if(user.admin){
      this.getBranches();
      return;  
    }
    
  }  

  getBranches() {
    let user: User = JSON.parse(localStorage.getItem('user'));

    this.branchService.getRestBranches('branch/getbranches').subscribe(response => {

      console.log(response);

      let result: Branch[] = response;

      if(user != null){
        if(user.admin){  
          for (let i = 0; i < result.length; i++) {            
            if(user.branchId.includes(result[i].id)){
              this.RestBranches.push(result[i]);
            }
          }      
        }
        else{
          this.RestBranches = response;
        }
      }
      else{
        this.RestBranches = response;
      }
    });
  }

  onClick(branch: Branch){
    //routerLinkNextPage
    let user: User = JSON.parse(localStorage.getItem('user'));

    if(user != null){
      this.referenceService.setBranch(branch.id);

      this.router.navigateByUrl('/menu/edit');

      return;
    }

    this.router.navigateByUrl('/menu/' + branch.id + "_clientE"); 
  }

  getStatus(branch: Branch): string{
    if(branch.lastActive < 30){
      return 'Online order available.'
    }
    
    return 'Online order unavailable';
  }
}
