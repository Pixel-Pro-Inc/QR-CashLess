import { __awaiter, __decorate } from "tslib";
import { Component } from '@angular/core';
let ThankyouComponent = class ThankyouComponent {
    constructor(router, referenceService) {
        this.router = router;
        this.referenceService = referenceService;
    }
    ngOnInit() {
        this.init();
    }
    init() {
        return __awaiter(this, void 0, void 0, function* () {
            yield this.sleep(10000);
            this.router.navigateByUrl('/menu/' + this.referenceService.currentBranch() + '_' + this.referenceService.currentreference());
        });
    }
    sleep(ms) {
        return new Promise((resolve) => {
            setTimeout(resolve, ms);
        });
    }
    navigateToSite(link) {
        window.open(link);
    }
};
ThankyouComponent = __decorate([
    Component({
        selector: 'app-thankyou',
        templateUrl: './thankyou.component.html',
        styleUrls: ['./thankyou.component.css']
    })
], ThankyouComponent);
export { ThankyouComponent };
//# sourceMappingURL=thankyou.component.js.map