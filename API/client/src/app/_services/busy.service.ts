import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseServiceService } from './-base-service.service';

@Injectable({
  providedIn: 'root'
})
export class BusyService extends BaseServiceService {

  busyRequestCount = 0;
  isLoading: Boolean;
  constructor(http: HttpClient) {
    super(http); }

  busy() {
    this.busyRequestCount++;    
  }
  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount<=0) {
      this.busyRequestCount = 0;
    }
  }

}
