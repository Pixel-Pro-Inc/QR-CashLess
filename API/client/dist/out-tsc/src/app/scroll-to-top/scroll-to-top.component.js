import { __decorate, __param } from "tslib";
import { Component, Inject, HostListener } from '@angular/core';
import { DOCUMENT } from '@angular/common';
let ScrollToTopComponent = class ScrollToTopComponent {
    constructor(document) {
        this.document = document;
    }
    onWindowScroll() {
        if (window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop > 100) {
            this.windowScrolled = true;
        }
        else if (this.windowScrolled && window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop < 10) {
            this.windowScrolled = false;
        }
    }
    scrollToTop() {
        document.body.scrollTop = 0; // For Safari
        document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
    }
    ngOnInit() { }
};
__decorate([
    HostListener("window:scroll", [])
], ScrollToTopComponent.prototype, "onWindowScroll", null);
ScrollToTopComponent = __decorate([
    Component({
        selector: 'app-scroll-to-top',
        templateUrl: './scroll-to-top.component.html',
        styleUrls: ['./scroll-to-top.component.css']
    }),
    __param(0, Inject(DOCUMENT))
], ScrollToTopComponent);
export { ScrollToTopComponent };
//# sourceMappingURL=scroll-to-top.component.js.map