import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { BaseServiceService } from './-base-service.service';
let MenuService = class MenuService extends BaseServiceService {
    constructor(http, busyService) {
        super(http);
        this.busyService = busyService;
    }
    getMenuItems(dir, branchId) {
        this.busyService.busy();
        return this.http.get(this.baseUrl + dir + '/' + branchId).pipe(map((response) => {
            this.busyService.idle();
            return response;
        }));
    }
    createMenuItem(dir, menuItemDto, branchId) {
        this.busyService.busy();
        return this.http.post(this.baseUrl + dir + '/' + branchId, menuItemDto).pipe(map((response) => {
            if (response) {
                localStorage.setItem(response.name, JSON.stringify(response)); //I changed it anyways, you migt not need to do it
            }
            this.busyService.idle();
            return response;
        }));
    }
    editMenuItem(dir, menuItemDto, branchId) {
        this.busyService.busy();
        return this.http.post(this.baseUrl + dir + '/' + branchId, menuItemDto).pipe(map((item) => {
            if (item) {
                localStorage.setItem(item.name, JSON.stringify(item));
            }
            this.busyService.idle();
            return item;
        }));
    }
    deleteItem(model, branchId) {
        this.busyService.busy();
        return this.http.post(this.baseUrl + 'menu/delete/' + branchId, model).pipe(map((item) => {
            this.busyService.idle();
            return item;
        }));
    }
    getSubCategories() {
        return this.http.get(this.baseUrl + 'menu/subcategory');
    }
    getFlavours() {
        return this.http.get(this.baseUrl + 'menu/flavours/get');
    }
    createFlavour(flavour) {
        this.http.get(this.baseUrl + 'menu/flavours/create/' + flavour).subscribe(response => {
            console.log(response);
            window.location.reload();
        });
    }
    getMeatTemperatures() {
        return this.http.get(this.baseUrl + 'menu/meattemperatures/get');
    }
    createMeatTemperature(meattemperature) {
        this.http.get(this.baseUrl + 'menu/meattemperature/create/' + meattemperature).subscribe(response => {
            console.log(response);
            window.location.reload();
        });
    }
    getSauces() {
        return this.http.get(this.baseUrl + 'menu/sauce/get');
    }
    createSauce(sauce) {
        this.http.get(this.baseUrl + 'menu/sauce/create/' + sauce).subscribe(response => {
            console.log(response);
            window.location.reload();
        });
    }
};
MenuService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], MenuService);
export { MenuService };
//# sourceMappingURL=menu.service.js.map