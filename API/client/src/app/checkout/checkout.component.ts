import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Branch } from '../_models/branch';
import { BranchService } from '../_services/branch.service';
import { ReferenceService } from '../_services/reference.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  branches: Branch[] = [];

  constructor(private branchService: BranchService) { }

  ngOnInit(): void {
    this.branchService.getRestBranches('branch/getbranches').subscribe(response => {
      let RestBranches: Branch[] = response;

      this.branches = RestBranches;      
    });
  }
  openingTime(b: Branch){
    return b.openingTime;
  }
  isOpen(b: Branch):Boolean{

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
  getBranch(){
    return localStorage.getItem('branch');
  }
}
