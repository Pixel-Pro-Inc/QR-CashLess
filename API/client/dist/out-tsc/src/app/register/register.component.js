import { __decorate } from "tslib";
import { Component, Output, EventEmitter } from '@angular/core';
let RegisterComponent = class RegisterComponent {
    constructor(accountService, toastr, branchService, referenceService) {
        this.accountService = accountService;
        this.toastr = toastr;
        this.branchService = branchService;
        this.referenceService = referenceService;
        this.cancelRegister = new EventEmitter();
        this.branches = [];
        this.model = {};
        this.isDeveloper = false;
    }
    ngOnInit() {
        this.accountService.currentUser$.subscribe(r => {
            this.user = r;
            this.isDeveloper = this.user.developer;
        });
        this.branchService.getRestBranches('branch/getbranches').subscribe(response => {
            this.branches = response;
        }, error => {
            console.log(error);
        });
    }
    register() {
        console.log(this.model);
        if (this.model.branchId == null) {
            let branch = [this.referenceService.currentBranch()];
            this.model.branchId = branch;
        }
        if (this.model.superUser) {
            this.resetBranches();
        }
        this.accountService.register(this.model, 'account/register').subscribe(response => {
            console.log(response);
            this.toastr.success("Your account was registered successfully");
            this.cancel();
        }, error => {
            console.log(error);
            this.toastr.error(error.error);
        });
    }
    cancel() {
        this.cancelRegister.emit(false);
    }
    resetBranches() {
        this.model.branchId = null;
    }
};
__decorate([
    Output()
], RegisterComponent.prototype, "cancelRegister", void 0);
RegisterComponent = __decorate([
    Component({
        selector: 'app-register',
        templateUrl: './register.component.html',
        styleUrls: ['./register.component.css']
    })
], RegisterComponent);
export { RegisterComponent };
//# sourceMappingURL=register.component.js.map