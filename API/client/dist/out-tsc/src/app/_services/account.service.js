import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { BaseServiceService } from './-base-service.service';
let AccountService = class AccountService extends BaseServiceService {
    constructor(http, busyService, router, toastr) {
        super(http);
        this.busyService = busyService;
        this.router = router;
        this.toastr = toastr;
        this.currentUserSource = new ReplaySubject(1);
        this.currentUser$ = this.currentUserSource.asObservable();
    }
    login(model, dir) {
        this.busyService.busy();
        return this.http.post(this.baseUrl + dir, model).subscribe(response => {
            let user = response;
            console.log(user);
            if (!user.admin && !user.developer && !user.superUser) {
                this.toastr.error('You cannot login to this platform');
                return null;
            }
            if (user != null) {
                localStorage.setItem('user', JSON.stringify(user));
                this.currentUserSource.next(user);
                this.router.navigateByUrl('/');
            }
            this.busyService.idle();
            return response;
        }, error => {
            this.toastr.error(error.error);
        });
    }
    request(model, dir) {
        this.busyService.busy();
        return this.http.post(this.baseUrl + dir, model, { responseType: 'text' }).subscribe(response => {
            this.busyService.idle();
            if (response == 'failed') {
                this.toastr.error("We could not find an account with those credentials.");
                return;
            }
            console.log(response);
            localStorage.setItem('resetToken', response);
            localStorage.setItem('accountID', model.accountID);
            this.router.navigateByUrl('/password/reset/success');
        });
    }
    reset(model, dir) {
        this.busyService.busy();
        this.http.post(this.baseUrl + dir, model, { responseType: 'text' }).subscribe(response => {
            this.busyService.idle();
            console.log(response);
            localStorage.removeItem('resetToken');
            localStorage.removeItem('accountID');
            this.toastr.success('Your password was successfully changed');
            this.router.navigateByUrl('login');
        });
    }
    register(model, dir) {
        this.busyService.busy();
        return this.http.post(this.baseUrl + dir, model).pipe(map((user) => {
            this.busyService.idle();
            return user;
        }));
    }
    setCurrentUser(mod) {
        this.currentUserSource.next(mod);
        localStorage.setItem('user', JSON.stringify(mod));
    }
    getCurrentUser() {
        return JSON.parse(localStorage.getItem('user'));
    }
    logout() {
        localStorage.removeItem('user');
        this.currentUserSource.next(null);
    }
};
AccountService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], AccountService);
export { AccountService };
//# sourceMappingURL=account.service.js.map