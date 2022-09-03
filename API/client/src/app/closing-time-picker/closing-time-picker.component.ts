import { Component, OnInit } from '@angular/core';
import { BranchService } from '../_services/branch.service';

@Component({
  selector: 'app-closing-time-picker',
  templateUrl: './closing-time-picker.component.html',
  styleUrls: ['./closing-time-picker.component.css']
})
export class ClosingTimePickerComponent implements OnInit {
  model: any = {};

  constructor(private branchService: BranchService) { }

  ngOnInit(): void {

  }

  setTime(){
    this.branchService.setBranchClosingTime(this.model);
  }

}
