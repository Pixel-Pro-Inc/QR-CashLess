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
  RestBranches: Branch[];

  title = 'Pick a restaurant branch most convenient to you';
  user: User;

  ngOnInit(): void {
    this.user = JSON.parse(localStorage.getItem('user'));// don't touch this line of code, this is the only place its set

    console.log(this.user);
    if(this.user == null){
      this.getBranches();
      let empty: OrderItem[] = [];
      localStorage.setItem('ordered', JSON.stringify(empty));
      return;
    }
    
    if(this.user.developer){
      this.getBranches();
      let empty: OrderItem[] = [];
      localStorage.setItem('ordered', JSON.stringify(empty));

      return;
    }

    if (this.user.admin && this.user != null/*||this.user.superUser==true*/){
      this.title = 'Choose a branch to manage';

      this.getBranches();      
    }
    
  }  

  getBranches() {
    this.branchService.getRestBranches('branch/getbranches').subscribe(response => {

      this.RestBranches = response;
      console.log("Before user was checked to be admin, we found that "+response);
      if (this.user == null) return; //there was a null reference error showing up and this solves it
      if(this.user.admin){
        console.log(response);

        for (let i = 0; i < this.RestBranches.length; i++) {
          console.log('entered');
          
          if(!(this.user.branchId.includes(this.RestBranches[i].id))){
            this.RestBranches.splice(i);
          }
        }      
      }
    });
  }

  onClick(branch: Branch){
    //routerLinkNextPage
    if (this.user != null) {

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
