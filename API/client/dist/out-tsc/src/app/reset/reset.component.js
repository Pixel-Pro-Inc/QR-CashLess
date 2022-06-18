import { __decorate } from "tslib";
import { Component } from '@angular/core';
let ResetComponent = class ResetComponent {
    constructor(account) {
        this.account = account;
        this.model = {};
    }
    ngOnInit() {
    }
    reset() {
        this.account.reset(this.model, 'account/forgotpassword/successful/' + localStorage.getItem('accountID') + '/' + this.model.password);
    }
    getToken() {
        return localStorage.getItem('resetToken');
    }
};
ResetComponent = __decorate([
    Component({
        selector: 'app-reset',
        templateUrl: './reset.component.html',
        styleUrls: ['./reset.component.css']
    })
], ResetComponent);
export { ResetComponent };
//# sourceMappingURL=reset.component.js.map