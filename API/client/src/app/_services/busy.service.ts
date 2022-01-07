import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseServiceService } from './-base-service.service';

@Injectable({
  providedIn: 'root'
})
export class BusyService extends BaseServiceService {

  busyRequestCount = 0;
  isLoading: Boolean;
  constructor(private spinnerService: NgxSpinnerService, http: HttpClient) {
    super(http); }

  busy() {
    this.busyRequestCount++;
    this.spinnerService.show(undefined,
      {
        type: 'Line-scale-party',
        bdColor: 'rgba(255,255,255,0)',
        color:'#33333333'
      }
    );
  }
  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount<=0) {
      this.busyRequestCount = 0;
      this.spinnerService.hide(undefined);
    }
  }

}
