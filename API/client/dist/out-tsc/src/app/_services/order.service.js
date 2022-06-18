import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { BaseServiceService } from './-base-service.service';
let OrderService = class OrderService extends BaseServiceService {
    constructor(http, router, referenceService, busyService) {
        super(http);
        this.router = router;
        this.referenceService = referenceService;
        this.busyService = busyService;
    }
    createOrder(orderItems) {
        this.busyService.busy();
        let bId = this.referenceService.currentBranch();
        this.http.post(this.baseUrl + 'order/createorder/' + bId, orderItems).subscribe(response => {
            localStorage.setItem('ordered', JSON.stringify(response));
            console.log(response);
            //Receipt Page
            if (this.referenceService.currentreference() != 'tablet') {
                this.router.navigateByUrl('/receipt');
                this.busyService.idle();
                return;
            }
            let x = '';
            x = this.referenceService.currentBranch() + '_' + this.referenceService.currentreference();
            let empty = [];
            localStorage.setItem('ordered', JSON.stringify(empty));
            if (this.referenceService.currentreference() == "tablet") {
                localStorage.setItem('userPhoneNumber', JSON.stringify(null));
            }
            this.busyService.idle();
            this.router.navigateByUrl('/thankyou');
        }, error => {
            console.log(error);
        });
    }
    paidOnlineForOrder(orderItems) {
        for (var i = 0; i < orderItems.length; i++) {
            if (!orderItems[i].purchased) {
                orderItems[i].purchased = true;
                orderItems[i].paymentMethod = 'online';
                orderItems[i].preparable = true;
            }
        }
        this.createOrder(orderItems);
    }
    payAtTill(orderItems) {
        for (var i = 0; i < orderItems.length; i++) {
            if (!orderItems[i].purchased) {
                orderItems[i].waitingForPayment = true;
                orderItems[i].preparable = true;
            }
        }
        this.createOrder(orderItems);
    }
};
OrderService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], OrderService);
export { OrderService };
//# sourceMappingURL=order.service.js.map