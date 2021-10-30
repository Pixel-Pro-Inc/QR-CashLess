import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { RestBranch } from '../_models/RestBranch';// I didn't set up any controllers for the API

@Component({
  selector: 'app-restaurant-branch',
  templateUrl: './restaurant-branch.component.html',
  styleUrls: ['./restaurant-branch.component.css']
})
export class RestaurantBranchComponent implements OnInit {

  @Output() RemoveRestSelect = new EventEmitter();
  constructor() { }

  ngOnInit(): void {
  }
  isActive: boolean=true;
  /*
  I tried working with some made up restbranches but to no avail

  Phakalane: RestBranch; Thamaga: RestBranch;
  names: RestBranch[] = new Array(this.Phakalane, this.Thamaga);

*/
  
  RestBranches: RestBranch[]; //needs info from the database

  isPOSactive(Rest: string)
  {
    for (var i = 0; i < this.RestBranches.length; i++) {
      if (Rest == this.RestBranches[i].name && this.RestBranches[i].active == true) {
        this.isActive = true;
      }
      else { this.isActive = false;}
    }
  }

  remove() {
    this.RemoveRestSelect.emit(false); //needs to be false
  }

}
