import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { RestBranch } from '../_models/RestBranch';// I didn't set up any controllers for the API
import { BranchesService } from '../_services/branches.service';

@Component({
  selector: 'app-restaurant-branch',
  templateUrl: './restaurant-branch.component.html',
  styleUrls: ['./restaurant-branch.component.css']
})
export class RestaurantBranchComponent implements OnInit {

  @Output() RemoveRestSelect = new EventEmitter();
  constructor(private branchService: BranchesService) { }
  model: any ={}

  ngOnInit(): void {
    this.getBranches();
    this.getStatuses(); //I'm leaving this here because there maybe a need to independently refresh the info on database
  }

  branchStatuses: boolean[];
  /*
  Nothing is showing up now cause there is nothing in the array, so its coming up null

*/
  
  RestBranches: RestBranch[]; //needs info from the database
  branchDictionary: Map<string, boolean>;

  getBranches() {
    this.branchService.getRestBranches('branch/getbranches').subscribe(response => {
      this.RestBranches = response;
      for (var i = 0; i < this.RestBranches.length; i++) {
        this.branchStatuses[i] = this.RestBranches[i].active;
      }
    }
    );
  }
  getStatuses() {
    for (var i = 0; i < this.RestBranches.length; i++) {
      this.branchDictionary.set(this.RestBranches[i].name,this.branchStatuses[i])
    }
  }
  remove() {
    this.RemoveRestSelect.emit(false); //needs to be false
  }

}
