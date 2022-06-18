import { __decorate } from "tslib";
import { Component } from '@angular/core';
let ResetpasswordComponent = class ResetpasswordComponent {
    constructor(account) {
        this.account = account;
        this.model = {};
    }
    ngOnInit() {
    }
    reset() {
        this.account.request(this.model, 'account/forgotpassword/' + this.model.accountID);
    }
};
ResetpasswordComponent = __decorate([
    Component({
        selector: 'app-resetpassword',
        templateUrl: './resetpassword.component.html',
        styleUrls: ['./resetpassword.component.css']
    })
], ResetpasswordComponent);
export { ResetpasswordComponent };
//# sourceMappingURL=resetpassword.component.js.map