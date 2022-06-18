import { __decorate } from "tslib";
import { Component } from '@angular/core';
let RestaurantBranchComponent = class RestaurantBranchComponent {
    constructor(branchService, router, referenceService) {
        this.branchService = branchService;
        this.router = router;
        this.referenceService = referenceService;
        this.RestBranches = [];
        this.title = 'Pick a restaurant branch most convenient to you';
    }
    ngOnInit() {
        this.referenceService.hideNavBar = true;
        let user;
        user = JSON.parse(localStorage.getItem('user'));
        let empty = [];
        localStorage.setItem('ordered', JSON.stringify(empty));
        if (user == null) {
            this.getBranches();
            return;
        }
        this.title = 'Choose a branch to manage';
        this.getBranches();
        return;
    }
    getBranches() {
        this.branchService.getRestBranches('branch/getbranches').subscribe(response => {
            console.log(response);
            let result = response;
            let user;
            user = JSON.parse(localStorage.getItem('user'));
            if (user != null) {
                if (user.admin) {
                    for (let i = 0; i < result.length; i++) {
                        if (user.branchId.includes(result[i].id)) {
                            this.RestBranches.push(result[i]);
                        }
                    }
                }
                else {
                    this.RestBranches = response;
                }
            }
            else {
                this.RestBranches = response;
            }
        });
    }
    onClick(branch) {
        //routerLinkNextPage
        let user;
        user = JSON.parse(localStorage.getItem('user'));
        if (user != null) {
            this.referenceService.setBranch(branch.id);
            this.referenceService.hideNavBar = false;
            this.router.navigateByUrl('/admin-portal');
            return;
        }
        //if (Usertype == 'tablet') return this.router.navigateByUrl('/menu/' + branch.id + "_tablet"); leave this here, cause tablets are never given user values
        //if (Usertype == 'QrCard') return this.router.navigateByUrl('/menu/' + branch.id + "_QrCard");
        //This here is where a param is attached and where it will be expected to have been done for the other options. Check how yewo did superUser so you can draw inspo for Usertype
        this.router.navigateByUrl('/menu/' + branch.id + "_clientE");
    }
    getStatus(branch) {
        if (!this.isOpen(branch)) {
            return 'Closed. Opens at ' + branch.openingTimeTomorrow;
        }
        if (branch.lastActive < 30) {
            return 'Online order available.';
        }
        return 'Online order unavailable';
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
};
RestaurantBranchComponent = __decorate([
    Component({
        selector: 'app-restaurant-branch',
        templateUrl: './restaurant-branch.component.html',
        styleUrls: ['./restaurant-branch.component.css']
    })
], RestaurantBranchComponent);
export { RestaurantBranchComponent };
//# sourceMappingURL=restaurant-branch.component.js.map