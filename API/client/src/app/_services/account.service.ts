import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccountService { 
  /** Services are singletons which means the dont destroy until the app isn't in use or u exit the broswer. COmponents get detroyed immediatly afteer use**/

  baseUrl = 'https://localhost:5001/api/';

  constructor(private Http: HttpClient) { }

  login(model: any) {
    return this.Http.post(this.baseUrl+'account/login', model)
  }

}
