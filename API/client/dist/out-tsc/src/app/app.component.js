import { __decorate } from "tslib";
import { Component } from '@angular/core';
import { NavigationEnd } from '@angular/router';
let AppComponent = class AppComponent {
    constructor(accountService, referenceService, route, router) {
        this.accountService = accountService;
        this.referenceService = referenceService;
        this.route = route;
        this.title = 'Rodizio Express';
        router.events.forEach((event) => {
            if (event instanceof NavigationEnd) {
                if (router.url.includes('menu') && !router.url.includes('edit')) {
                    this.showViewOrder = true;
                }
                else {
                    this.showViewOrder = false;
                }
            }
        });
    }
    ngOnInit() {
        this.setCurrentUser();
        console.log(this.referenceService.currentreference());
        this.SelectorMode = true;
    }
    setCurrentUser() {
        const user = JSON.parse(localStorage.getItem('user'));
        this.accountService.setCurrentUser(user);
    }
    navigateToSite(link) {
        window.open(link);
    }
    RemoveSelector(event) {
        this.SelectorMode = event;
    }
};
AppComponent = __decorate([
    Component({
        selector: 'app-root',
        templateUrl: './app.component.html',
        styleUrls: ['./app.component.css']
    })
], AppComponent);
export { AppComponent };
//# sourceMappingURL=app.component.js.map