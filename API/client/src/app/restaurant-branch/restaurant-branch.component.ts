import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Branch } from '../_models/branch';
import { OrderItem } from '../_models/orderItem';
import { BranchService } from '../_services/branch.service';

@Component({
  selector: 'app-restaurant-branch',
  templateUrl: './restaurant-branch.component.html',
  styleUrls: ['./restaurant-branch.component.css']
})
export class RestaurantBranchComponent implements OnInit {
  constructor(private branchService: BranchService, private router: Router) { }
  RestBranches: Branch[];

  ngOnInit(): void {
    this.getBranches();
  }  

  getBranches() {
    this.branchService.getRestBranches('branch/getbranches').subscribe(response => {
      this.RestBranches = response;
    });
  }

  onClick(branch: Branch){
    //routerLinkNextPage
    this.router.navigateByUrl('/menu/:' + branch.id );
  }

  getStatus(branch: Branch): string{
    if(branch.lastActive < 30){
      return 'Online order available.'
    }
    
    return 'Online order unavailable';
  }
}
