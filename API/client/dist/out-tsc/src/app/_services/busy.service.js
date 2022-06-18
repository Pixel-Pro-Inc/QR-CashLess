import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
let BusyService = class BusyService {
    constructor(spinnerService) {
        this.spinnerService = spinnerService;
        this.busyRequestCount = 0;
    }
    busy() {
        /*this.busyRequestCount++;
        this.spinnerService.show(undefined, {
          type: 'ball-spin-clockwise-fade-rotating',
          bdColor: 'rgba(0, 0, 0, 0.8)',
          color: '#fff'
        });*/
    }
    idle() {
        /*this.busyRequestCount--;
        if(this.busyRequestCount <= 0){
          this.busyRequestCount = 0;
          this.spinnerService.hide();
        }*/
    }
    busy_1() {
        this.busyRequestCount++;
        this.spinnerService.show(undefined, {
            type: 'ball-clip-rotate',
            bdColor: 'rgba(0, 0, 0, 0.8)',
            color: '#fff'
        });
    }
    idle_1() {
        this.busyRequestCount--;
        if (this.busyRequestCount <= 0) {
            this.busyRequestCount = 0;
            this.spinnerService.hide();
        }
    }
};
BusyService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], BusyService);
export { BusyService };
//# sourceMappingURL=busy.service.js.map