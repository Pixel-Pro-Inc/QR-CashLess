import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
let BaseServiceService = class BaseServiceService {
    constructor(http) {
        this.http = http;
        /**
         *this needs to be public so anything that uses it, eg components, will take from it. Im confident its safe cause all the services are private properties injected into
         * components anyways
         */
        // REFACTOR: Here is a good place to use an environment variable so we don't have future problems
        this.baseUrl = 'https://rodizioexpress.com/api/';
    }
};
BaseServiceService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], BaseServiceService);
export { BaseServiceService };
//# sourceMappingURL=-base-service.service.js.map