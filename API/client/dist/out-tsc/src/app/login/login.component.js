import { __decorate } from "tslib";
import { Component } from '@angular/core';
import { Validators } from '@angular/forms';
let LoginComponent = class LoginComponent {
    constructor(fb, accountService) {
        this.fb = fb;
        this.accountService = accountService;
    }
    ngOnInit() {
        this.initializeForm();
    }
    initializeForm() {
        this.signinForm = this.fb.group({
            username: ['', [Validators.required]],
            password: ['', [Validators.required]]
        });
    }
    signin() {
        this.accountService.login(this.signinForm.value, 'account/login');
    }
};
LoginComponent = __decorate([
    Component({
        selector: 'app-login',
        templateUrl: './login.component.html',
        styleUrls: ['./login.component.css']
    })
], LoginComponent);
export { LoginComponent };
//# sourceMappingURL=login.component.js.map