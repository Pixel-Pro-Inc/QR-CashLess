import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { BaseServiceService } from './-base-service.service';
let BranchService = class BranchService extends BaseServiceService {
    constructor(http, toastr, busy) {
        super(http);
        this.toastr = toastr;
        this.busy = busy;
    }
    submission(model, dir) {
        this.busy.busy();
        return this.http.post(this.baseUrl + dir, model).subscribe(response => {
            this.toastr.success('Branch registered successfully');
            this.busy.idle();
            return response;
        }, error => {
            console.log(error);
            this.busy.idle();
        });
    }
    getRestBranches(dir) {
        this.busy.busy();
        return this.http.get(this.baseUrl + dir).pipe(map((response) => {
            this.busy.idle();
            return response;
        }));
    }
    setBranchClosingTime(model) {
        this.http.post(this.baseUrl + 'branch/setclosingtime', model).subscribe(response => {
            this.toastr.success("Branch Closing Time Set Successfully");
        });
    }
};
BranchService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], BranchService);
export { BranchService };
//# sourceMappingURL=branch.service.js.map