import { __decorate } from "tslib";
import { Component, Input } from '@angular/core';
let QuantityCounterComponent = class QuantityCounterComponent {
    constructor() {
        this.model = {};
    }
    ngOnInit() {
        this.model.quantity = 1;
    }
    minus() {
        if (this.model.quantity != 1) {
            this.model.quantity--;
        }
    }
    plus() {
        this.model.quantity++;
        this.display();
    }
    display() {
        console.log(this.model);
    }
};
__decorate([
    Input()
], QuantityCounterComponent.prototype, "model", void 0);
QuantityCounterComponent = __decorate([
    Component({
        selector: 'app-quantity-counter',
        templateUrl: './quantity-counter.component.html',
        styleUrls: ['./quantity-counter.component.css']
    })
], QuantityCounterComponent);
export { QuantityCounterComponent };
//# sourceMappingURL=quantity-counter.component.js.map