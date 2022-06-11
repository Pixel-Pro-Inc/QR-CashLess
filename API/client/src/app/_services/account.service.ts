import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Toast, ToastrService } from 'ngx-toastr';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { AdminUser } from '../_models/billeduser';
import { User } from '../_models/user';
import { BaseServiceService } from './-base-service.service';
import { BusyService } from './busy.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService extends BaseServiceService{

  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(http: HttpClient, public busyService: BusyService, private router: Router, private toastr: ToastrService) {
  // I need to use the developer boolean in several components so its only smart to use it in a service
  developer = false;

  constructor(http: HttpClient) {
    super(http);
  }

  login(model: any, dir: string) {
    this.busyService.busy();

    return this.http.post(this.baseUrl + dir, model).subscribe(
      response => {
        let user: any = response;

        console.log(user);

        if(!user.admin && !user.developer && !user.superUser){
          this.toastr.error('You cannot login to this platform');
          return null;
        }

        if(user != null) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
          this.router.navigateByUrl('/');
        }

        this.busyService.idle();
        return response;

      },
      error => {
        this.toastr.error(error.error);
      }
    )
  }

  request(model: any, dir: string) {
    this.busyService.busy();

    return this.http.post(this.baseUrl + dir, model, {responseType: 'text'}).subscribe(
      response => {
        this.busyService.idle();

        if(response == 'failed'){
          this.toastr.error("We could not find an account with those credentials.");
          return;
        }

        console.log(response);

        localStorage.setItem('resetToken', response);
        localStorage.setItem('accountID', model.accountID);

        this.router.navigateByUrl('/password/reset/success');
      }
    );
  }
  
  reset(model: any, dir: string) {
    this.busyService.busy();

    this.http.post(this.baseUrl + dir, model, {responseType: 'text'}).subscribe(
      response => {
        this.busyService.idle();

        console.log(response);

        localStorage.removeItem('resetToken');
        localStorage.removeItem('accountID');

        this.toastr.success('Your password was successfully changed');
        
        this.router.navigateByUrl('login');
      }
    );
  }

  register(model: any, dir: string) {
    this.busyService.busy();
    return this.http.post(this.baseUrl + dir, model).pipe(
      map((user: User) => {
        this.busyService.idle();
        return user;
      })
    )
  }

  setCurrentUser(mod: User){
    this.currentUserSource.next(mod);
    localStorage.setItem('user', JSON.stringify(mod));
  }

  getCurrentUser(){
    return JSON.parse(localStorage.getItem('user'));
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
  
  /**
   * Try to get the Admin Users so that we can chose from there as developers which ones can be billed
   * @returns list of @see AdminUser
   */
  getAdminUsers(){
    
    return this.http.get(this.baseUrl + "account/getadminusers").pipe(
      map((response: AdminUser[]) => {
        return response;
      })
    );
  }

}
