import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private account: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    if(this.account.getCurrentUser() != null){
      let token = this.account.getCurrentUser().token;

      request = request.clone({
        setHeaders:{
          Authorization: `Bearer ${token}`
        }
      });
    }    

    return next.handle(request);
  }
}
