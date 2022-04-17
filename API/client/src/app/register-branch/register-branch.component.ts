import { Time } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Branch } from '../_models/branch';
import { BranchService } from '../_services/branch.service';


@Component({
  selector: 'app-register-branch',
  templateUrl: './register-branch.component.html',
  styleUrls: ['./register-branch.component.css']
})
export class RegisterBranchComponent implements OnInit {
  dto: any = {};
  img: any;

  constructor(private branchService: BranchService) { }

  ngOnInit(): void {
  }

  register(){
    let n = Math.random() * 100000;
    n = Math.round(n);
    this.dto.id = 'rd' + n;

    //Set Phonenumbers
    let arrayPhoneNumber: string[] = [this.dto.phoneNumber1, this.dto.phoneNumber2];
    this.dto.PhoneNumbers = arrayPhoneNumber;

    //Set Times
    let arrayOpenTimes: Time[] = [this.dto.openingTime_1, this.dto.openingTime_2, this.dto.openingTime_3, this.dto.openingTime_4, 
      this.dto.openingTime_5, this.dto.openingTime_6, this.dto.openingTime_7, this.dto.openingTime_8];
    this.dto.OpeningTime = arrayOpenTimes;

    let arrayClosingTimes: Time[] = [this.dto.closingTime_1, this.dto.closingTime_2, this.dto.closingTime_3, this.dto.closingTime_4,
      this.dto.closingTime_5, this.dto.closingTime_6, this.dto.closingTime_7, this.dto.closingTime_8];

    this.dto.ClosingTime = arrayClosingTimes;
    
    console.log(this.dto);

    this.branchService.submission(this.dto, 'branch/register');
    
    this.ngOnInit;
  }

  onFileChange(event) {
    const reader = new FileReader();

    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      reader.readAsDataURL(file);

      reader.onload = () => {

        this.dto.img = reader.result as string;
      };

    }
  }

}
