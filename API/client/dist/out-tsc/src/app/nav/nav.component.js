import { __decorate } from "tslib";
import { Component } from '@angular/core';
let NavComponent = class NavComponent {
    constructor(referenceService, accountService, router, toastr) {
        this.referenceService = referenceService;
        this.accountService = accountService;
        this.router = router;
        this.toastr = toastr;
        this.admin = false;
        this.developer = false;
        this.kitchenStaff = false;
    }
    ngOnInit() {
        this.accountService.currentUser$.subscribe(response => {
            this.user = response;
            if (this.user != null) {
                if (this.user.admin) {
                    this.admin = true;
                }
                else if (this.user.developer) {
                    this.developer = true;
                }
                else {
                    this.kitchenStaff = true;
                }
            }
        });
    }
    logout() {
        this.accountService.logout();
        this.router.navigateByUrl('/login');
    }
};
NavComponent = __decorate([
    Component({
        selector: 'app-nav',
        templateUrl: './nav.component.html',
        styleUrls: ['./nav.component.css']
    })
], NavComponent);
export { NavComponent };
//# sourceMappingURL=nav.component.js.map