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
    let user: User;
    user = JSON.parse(localStorage.getItem('user'));// don't touch this line of code, this is the only place its set

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
    this.branchService.getRestBranches('branch/getbranches').subscribe(response => {

      console.log(response);

      let result: Branch[] = response;
      let user: User;

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
    let user: User;
    if (user != null) {

      this.referenceService.setBranch(branch.id);

      this.router.navigateByUrl('/menu/edit');

      return;
    }

    //if (Usertype == 'tablet') return this.router.navigateByUrl('/menu/' + branch.id + "_tablet"); leave this here, cause tablets are never given user values
    //if (Usertype == 'QrCard') return this.router.navigateByUrl('/menu/' + branch.id + "_QrCard");
    // This here is where a param is attached and where it will be expected to have been done for the other options. Check how yewo did superUser so you can draw inspo for Usertype
    this.router.navigateByUrl('/menu/' + branch.id + "_clientE"); 
  }

  getStatus(branch: Branch): string{
    if(branch.lastActive < 30){
      return 'Online order available.'
    }
    
    return 'Online order unavailable';
  }
}
