import { __decorate, __param } from "tslib";
import { Component, Input, Self } from '@angular/core';
let TextInputComponent = class TextInputComponent {
    constructor(ngControl) {
        this.ngControl = ngControl;
        this.type = 'text';
        this.placeholder = '';
        this.id = '';
        this.ngControl.valueAccessor = this;
    }
    writeValue(obj) {
    }
    registerOnChange(fn) {
    }
    registerOnTouched(fn) {
    }
};
__decorate([
    Input()
], TextInputComponent.prototype, "label", void 0);
__decorate([
    Input()
], TextInputComponent.prototype, "type", void 0);
__decorate([
    Input()
], TextInputComponent.prototype, "placeholder", void 0);
__decorate([
    Input()
], TextInputComponent.prototype, "id", void 0);
TextInputComponent = __decorate([
    Component({
        selector: 'app-text-input',
        templateUrl: './text-input.component.html',
        styleUrls: ['./text-input.component.css']
    }),
    __param(0, Self())
], TextInputComponent);
export { TextInputComponent };
//# sourceMappingURL=text-input.component.js.map