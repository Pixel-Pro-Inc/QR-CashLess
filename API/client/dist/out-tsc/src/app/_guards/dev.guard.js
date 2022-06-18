import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
let DevGuard = class DevGuard {
    constructor(accountService, toastr) {
        this.accountService = accountService;
        this.toastr = toastr;
    }
    canActivate() {
        return this.accountService.currentUser$.pipe(map(user => {
            if (user) {
                if (user.developer)
                    return true;
            }
            console.log(user);
            this.toastr.error('You are unauthorized!');
        }));
    }
};
DevGuard = __decorate([
    Injectable({
        providedIn: 'root'
    })
], DevGuard);
export { DevGuard };
//# sourceMappingURL=dev.guard.js.map