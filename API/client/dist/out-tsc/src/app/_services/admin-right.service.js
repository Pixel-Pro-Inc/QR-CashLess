import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { BaseServiceService } from './-base-service.service';
let AdminRightService = class AdminRightService extends BaseServiceService {
    constructor(http) {
        super(http);
        this.getbranches = () => this.branchClass.getBranches();
        this.getreceipt = (slipNumber) => this.recieptDictionary.get(slipNumber);
    }
    deleteMenuItem(model, dir) {
        return this.http.post(this.baseUrl + dir, model).pipe(map((item) => {
            if (item) {
                localStorage.removeItem(item.name);
            }
            return item;
        }));
    }
    editMenuItem(model, dir) {
        return this.http.post(this.baseUrl + dir, model).pipe(map((item) => {
            if (item) {
                localStorage.setItem(item.name, JSON.stringify(item)); //I'm assuming that since its the same place and name it will just overwrite
            }
            return item;
        }));
    }
    pushtoReceiptBucket(model, dir) {
        return this.http.post(this.baseUrl + dir, model).pipe(map((sliplist) => {
            if (sliplist) {
                localStorage.setItem(Date.now.toString(), JSON.stringify(sliplist)); //I'm using date here cause its a batch of receipts with different times, so its better to identify them when they are sent up
            }
            return sliplist;
        }));
    }
    getreceiptList(model, dir) {
        return this.http.get(this.baseUrl + dir).pipe(map((response) => {
            return response;
        }));
    }
};
AdminRightService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], AdminRightService);
export { AdminRightService };
//# sourceMappingURL=admin-right.service.js.map