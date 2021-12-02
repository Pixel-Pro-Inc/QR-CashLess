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

  model: Branch = {
    name: '',
    id: '',
    location: '',
    img: '',
    lastActive: undefined,
    phoneNumber: 0
  };
  img: any;

  constructor(private branchService: BranchService) { }

  ngOnInit(): void {
  }

  register(){
    let n = Math.random() * 100000;
    n = Math.round(n);
    this.model.id = 'rd'+ n;
    
    this.branchService.submission(this.model, 'branch/register');
    
    this.ngOnInit;
  }

  onFileChange(event) {
    const reader = new FileReader();

    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      reader.readAsDataURL(file);

      reader.onload = () => {

        this.model.img = reader.result as string;
      };

    }
  }

}
