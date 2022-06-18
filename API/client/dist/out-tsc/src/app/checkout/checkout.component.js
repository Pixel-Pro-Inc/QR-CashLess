import { __decorate } from "tslib";
import { Component } from '@angular/core';
let CheckoutComponent = class CheckoutComponent {
    constructor(branchService) {
        this.branchService = branchService;
        this.branches = [];
    }
    ngOnInit() {
        this.branchService.getRestBranches('branch/getbranches').subscribe(response => {
            let RestBranches = response;
            this.branches = RestBranches;
        });
    }
    openingTime(b) {
        return b.openingTime;
    }
    isOpen(b) {
        let currentTime = new Date();
        let times = this.SetHoursMins(b);
        let cH = currentTime.getHours();
        if (cH > times[0].hours) {
            return false;
        }
        if (cH < times[1].hours) {
            return false;
        }
        let cM = currentTime.getMinutes();
        if (cH == times[1].hours) {
            if (cM < times[1].minutes) {
                return false;
            }
        }
        if (cH == times[0].hours) {
            if (cM > times[0].minutes) {
                return false;
            }
        }
        return true;
    }
    SetHoursMins(branch) {
        let tString = branch.closingTime.toString();
        let h = parseInt(tString.substring(0, 2));
        let m = parseInt(tString.substring(3, 5));
        let times = [];
        let tC = {};
        tC.hours = h;
        tC.minutes = m;
        times.push(tC);
        tString = branch.openingTime.toString();
        h = parseInt(tString.substring(0, 2));
        m = parseInt(tString.substring(3, 5));
        let tO = {};
        tO.hours = h;
        tO.minutes = m;
        times.push(tO);
        return times;
    }
    getBranch() {
        return localStorage.getItem('branch');
    }
};
CheckoutComponent = __decorate([
    Component({
        selector: 'app-checkout',
        templateUrl: './checkout.component.html',
        styleUrls: ['./checkout.component.css']
    })
], CheckoutComponent);
export { CheckoutComponent };
//# sourceMappingURL=checkout.component.js.map