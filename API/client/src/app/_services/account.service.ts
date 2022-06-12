import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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
    super(http);
  }
  // I need to use the developer and superUser boolean in several components so its only smart to use it in a service
  developer = false;
  superUser=false;


  login(model: any, dir: string) {
    return this.http.post(this.baseUrl + dir, model).pipe(
      map((response: User) => {
        const user = response;

        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
        return response;
      })
    )
  }
  
  register(model: any, dir: string) {
    return this.http.post(this.baseUrl + dir, model).pipe(
      map((user: User) => {
        return user;
      })
    )
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
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
