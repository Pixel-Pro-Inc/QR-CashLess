import { __decorate } from "tslib";
import { Component } from '@angular/core';
let SideBarComponent = class SideBarComponent {
    constructor() { }
    ngOnInit() {
    }
    ToggleSideBar() {
        var sidebar = document.getElementById('sidebar');
        sidebar.classList.toggle('open');
    }
};
SideBarComponent = __decorate([
    Component({
        selector: 'app-side-bar',
        templateUrl: './side-bar.component.html',
        styleUrls: ['./side-bar.component.css']
    })
], SideBarComponent);
export { SideBarComponent };
//# sourceMappingURL=side-bar.component.js.map