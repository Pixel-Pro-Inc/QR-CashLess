import { __decorate } from "tslib";
import { Component } from '@angular/core';
import { take } from 'rxjs/operators';
let PhotouploaderComponent = class PhotouploaderComponent {
    constructor(accountService) {
        this.accountService = accountService;
        this.baseUrl = '';
        this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    }
    ngOnInit() {
        this.baseUrl = this.accountService.baseUrl;
        this.initializeUploader();
    }
    initializeUploader() {
    }
};
PhotouploaderComponent = __decorate([
    Component({
        selector: 'app-photouploader',
        templateUrl: './photouploader.component.html',
        styleUrls: ['./photouploader.component.css']
    })
], PhotouploaderComponent);
export { PhotouploaderComponent };
//# sourceMappingURL=photouploader.component.js.map