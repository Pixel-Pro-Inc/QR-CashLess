import { __decorate } from "tslib";
import { Component } from '@angular/core';
import { ReceiptComponent } from '../receipt/receipt.component';
let AdminComponent = class AdminComponent {
    constructor(accountService, router, toastr, adminPower) {
        this.accountService = accountService;
        this.router = router;
        this.toastr = toastr;
        this.adminPower = adminPower;
        this.model = {};
    }
    ngOnInit() {
    }
    pushreceipt(model) {
        if (model == typeof (ReceiptComponent)) {
            //this.adminPower.pushtoReceiptDic(model);
        }
        else {
            console.log('wrong type');
        }
    }
    getreceipt(slipNumber) {
        this.adminPower.getreceipt(slipNumber);
    }
    pushReceiptList() {
        this.adminPower.pushtoReceiptBucket('receipt/pushreceipts', this.model).subscribe(response => {
            console.log(response);
            window.location.reload();
        });
    }
    pullReceiptList() {
        this.adminPower.getreceiptList('receipt/pullreceipts', this.model).subscribe(response => {
            console.log(response);
            window.location.reload();
        });
    }
};
AdminComponent = __decorate([
    Component({
        selector: 'app-admin',
        templateUrl: './admin.component.html',
        styleUrls: ['./admin.component.css']
    })
], AdminComponent);
export { AdminComponent };
//# sourceMappingURL=admin.component.js.map