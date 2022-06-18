import { __decorate } from "tslib";
import { Component } from '@angular/core';
let ClosingTimePickerComponent = class ClosingTimePickerComponent {
    constructor(branchService) {
        this.branchService = branchService;
        this.model = {};
    }
    ngOnInit() {
    }
    setTime() {
        this.branchService.setBranchClosingTime(this.model);
    }
};
ClosingTimePickerComponent = __decorate([
    Component({
        selector: 'app-closing-time-picker',
        templateUrl: './closing-time-picker.component.html',
        styleUrls: ['./closing-time-picker.component.css']
    })
], ClosingTimePickerComponent);
export { ClosingTimePickerComponent };
//# sourceMappingURL=closing-time-picker.component.js.map