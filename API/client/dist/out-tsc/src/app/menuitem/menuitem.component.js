import { __decorate } from "tslib";
import { Component, Input } from '@angular/core';
let MenuitemComponent = class MenuitemComponent {
    constructor(toastr, menuService, referenceService) {
        this.toastr = toastr;
        this.menuService = menuService;
        this.referenceService = referenceService;
        this.subCategory = '';
        this.model = {};
        this.userInput = [];
    }
    ngOnInit() {
    }
    clicked(item, usersInput) {
        console.log(item);
        console.log(usersInput);
        console.log(this.model);
        console.log(item);
        this.toastr.success(item.name + ' was added to your order.', 'Order Confirmation', {
            positionClass: 'toast-top-right'
        });
        this.orderView.updateOrderView(item, this.model.quantity, usersInput);
    }
    editItem(model) {
        this.menuView.showEditForm = true;
        this.menuView.model1 = model;
        console.log(model);
        this.toBottom();
    }
    toBottom() {
        window.scrollTo(0, document.body.scrollHeight);
    }
    deleteItem(model) {
        if (confirm("Are you sure you want to permanently delete " + model.name)) {
            this.menuService.deleteItem(model, this.referenceService.currentBranch()).subscribe(response => {
                window.location.reload();
            });
        }
    }
};
__decorate([
    Input()
], MenuitemComponent.prototype, "menuItems", void 0);
__decorate([
    Input()
], MenuitemComponent.prototype, "category", void 0);
__decorate([
    Input()
], MenuitemComponent.prototype, "subCategory", void 0);
__decorate([
    Input()
], MenuitemComponent.prototype, "cantOrder", void 0);
__decorate([
    Input()
], MenuitemComponent.prototype, "orderView", void 0);
__decorate([
    Input()
], MenuitemComponent.prototype, "menuView", void 0);
MenuitemComponent = __decorate([
    Component({
        selector: 'app-menuitem',
        templateUrl: './menuitem.component.html',
        styleUrls: ['./menuitem.component.css']
    })
], MenuitemComponent);
export { MenuitemComponent };
//# sourceMappingURL=menuitem.component.js.map