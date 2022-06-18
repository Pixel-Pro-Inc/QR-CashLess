import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { BaseServiceService } from './-base-service.service';
let ReferenceService = class ReferenceService extends BaseServiceService {
    constructor(http) {
        super(http);
        this.hideNavBar = false;
    }
    currentreference() {
        return localStorage.getItem('reference');
    }
    currentBranch() {
        return localStorage.getItem('branch');
    }
    /**
     * so the set reference is used in Menu component line:59
     * @param param
     * are set in restaurant Branch onClick()
     */
    setReference(param) {
        console.log("This is the params " + param);
        let indexOfBreak = param.indexOf('_');
        let ref = param.slice(indexOfBreak + 1, param.length);
        console.log("This is the reference " + ref);
        let branch = param.slice(0, indexOfBreak);
        console.log("This is the branch " + branch);
        localStorage.setItem('reference', ref);
        this.setBranch(branch);
    }
    setRefExplicit(ref) {
        localStorage.setItem('reference', ref);
    }
    setBranch(branch) {
        localStorage.setItem('branch', branch);
    }
};
ReferenceService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], ReferenceService);
export { ReferenceService };
//# sourceMappingURL=reference.service.js.map