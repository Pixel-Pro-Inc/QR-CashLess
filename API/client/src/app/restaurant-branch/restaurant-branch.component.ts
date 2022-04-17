import { Time } from '@angular/common';
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
    this.referenceService.hideNavBar = true;

    let user: User;
    user = JSON.parse(localStorage.getItem('user'));

    let empty: OrderItem[] = [];
    localStorage.setItem('ordered', JSON.stringify(empty));

    if(user == null){
      this.getBranches();
      return;
    }

    this.title = 'Choose a branch to manage';

    this.getBranches();
      return;    
  }  

  getBranches() {
    this.branchService.getRestBranches('branch/getbranches').subscribe(response => {

      console.log(response);

      let result: Branch[] = response;
      
      let user: User;
      user = JSON.parse(localStorage.getItem('user'));

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
    user = JSON.parse(localStorage.getItem('user'));
    if (user != null) {

      this.referenceService.setBranch(branch.id);
      this.referenceService.hideNavBar = false;

      this.router.navigateByUrl('/dashboard');

      return;
    }

    //if (Usertype == 'tablet') return this.router.navigateByUrl('/menu/' + branch.id + "_tablet"); leave this here, cause tablets are never given user values
    //if (Usertype == 'QrCard') return this.router.navigateByUrl('/menu/' + branch.id + "_QrCard");
    //This here is where a param is attached and where it will be expected to have been done for the other options. Check how yewo did superUser so you can draw inspo for Usertype
    this.router.navigateByUrl('/menu/' + branch.id + "_clientE");
  }

  getStatus(branch: Branch): string{
    if(!this.isOpen(branch)){
      return 'Closed. Opens at ' + branch.openingTimeTomorrow;
    }

    if(branch.lastActive < 30){
      return 'Online order available.'
    } 
    
    return 'Online order unavailable';
  }

  isOpen (b: Branch):Boolean{
    let currentTime = new Date();

    let times:any[] = this.SetHoursMins(b);

    let cH = currentTime.getHours();

    if(cH > times[0].hours){
      return false;
    }

    if(cH < times[1].hours){
      return false;
    }

    let cM = currentTime.getMinutes();

    if(cH == times[1].hours){
      if(cM < times[1].minutes){
        return false;
      }
    }

    if(cH == times[0].hours){
      if(cM > times[0].minutes){
        return false;
      }
    }

    return true;
  }

  SetHoursMins(branch: Branch){
    let tString = branch.closingTime.toString();

    let h: number = parseInt(tString.substring(0, 2));

    let m: number = parseInt(tString.substring(3, 5));

    let times: any = [];

    let tC: any = {};

    tC.hours = h;

    tC.minutes = m;

    times.push(tC);

    tString = branch.openingTime.toString();

    h = parseInt(tString.substring(0, 2));

    m = parseInt(tString.substring(3, 5));

    let tO: any = {};

    tO.hours = h;

    tO.minutes = m;

    times.push(tO);

    return times;
  }
}
