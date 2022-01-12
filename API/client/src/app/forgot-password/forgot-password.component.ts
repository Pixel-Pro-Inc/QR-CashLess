import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { forgotPassword } from '../_models/forgotPassword';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {

  public forgotPasswordForm: FormGroup
  public successMessage: string;
  public errorMessage: string;
  public showSuccess: boolean;
  public showError: boolean;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.forgotPasswordForm = new FormGroup({
      phoneNumber: new FormControl("", [Validators.required])
    })
  }
  public validateControl = (controlName: string) => {
    return this.forgotPasswordForm.controls[controlName].invalid && this.forgotPasswordForm.controls[controlName].touched
    //I don't know how they validate this, yet
  }
  public hasError = (controlName: string, errorName: string) => {
    return this.forgotPasswordForm.controls[controlName].hasError(errorName)
  }
  public forgotPassword = (forgotPasswordFormValue) => {
    this.showError = this.showSuccess = false;
    const forgotPass = { ...forgotPasswordFormValue };
    const forgotPassDto: forgotPassword = {
      username:forgotPass.username,
      phoneNumber: forgotPass.phoneNumber,
      clientURI: 'http://localhost:4200/resetpassword'
    }
    this.accountService.forgotPassword('/accounts/forgotpassword', forgotPassDto)
      .subscribe(_ => {
        this.showSuccess = true;
        this.successMessage = 'The link has been sent, please check your SMS to reset your password.'
      },
        err => {
          this.showError = true;
          this.errorMessage = err;
        })
  }
}
