import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { finalize } from 'rxjs/operators';
let LoadingInterceptor = class LoadingInterceptor {
    constructor(busyService) {
        this.busyService = busyService;
    }
    intercept(request, next) {
        this.busyService.busy();
        //console.log("So busy was called by the interceptor")
        return next.handle(request).pipe(finalize(() => {
            this.busyService.idle();
        }));
    }
};
LoadingInterceptor = __decorate([
    Injectable()
], LoadingInterceptor);
export { LoadingInterceptor };
//# sourceMappingURL=loading.interceptor.js.map