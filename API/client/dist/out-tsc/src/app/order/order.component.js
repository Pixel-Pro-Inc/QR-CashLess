import { __decorate } from "tslib";
import { Component, Input } from '@angular/core';
let OrderComponent = class OrderComponent {
    constructor(menuService, router, orderService, referenceService, branchService) {
        this.menuService = menuService;
        this.router = router;
        this.orderService = orderService;
        this.referenceService = referenceService;
        this.branchService = branchService;
        this.total = 0.00;
        this.totalDisplay = '';
        this.totalUsd = 0.00;
        this.showInteractionOptions = {};
        this.tempOrderItems = [];
        this.menuItems = [];
        this.model = {};
        this.block = 0;
    }
    ngOnInit() {
        this.showInteractionOptions = null;
        this.model.phoneNumber = JSON.parse(localStorage.getItem('userPhoneNumber'));
        this.orderItems = this.getOrders();
        this.getMenuItems(this.referenceService.currentBranch());
        this.onlineOrderAvailable();
    }
    onlineOrderAvailable() {
        this.branchService.getRestBranches('branch/getbranches').subscribe(response => {
            console.log(response);
            let RestBranches = response;
            for (let i = 0; i < RestBranches.length; i++) {
                if (RestBranches[i].id == this.referenceService.currentBranch()) {
                    console.log(RestBranches[i].phoneNumbers);
                    this.branchPhoneNumbers = RestBranches[i].phoneNumbers;
                    if (RestBranches[i].lastActive < 30 && this.isOpen(RestBranches[i])) {
                        this.showInteractionOptions = true;
                    }
                }
            }
            this.showInteractionOptions = false;
        });
    }
    getMenuItems(branchId) {
        //console.log(branchId);
        this.menuService.getMenuItems('menu/getmenu', branchId).subscribe(response => {
            this.menuItems = response;
            this.menuItems.forEach(element => {
                element.selectedFlavour = 'None';
                element.selectedMeatTemperature = 'Well Done';
                element.selectedSauces = ['Lemon & Garlic'];
            });
            console.log(response);
        }, error => console.log(error));
    }
    isOpen(b) {
        let currentTime = new Date();
        let times = this.SetHoursMins(b);
        let cH = currentTime.getHours();
        if (cH > times[0].hours) {
            return false;
        }
        if (cH < times[1].hours) {
            return false;
        }
        let cM = currentTime.getMinutes();
        if (cH == times[1].hours) {
            if (cM < times[1].minutes) {
                return false;
            }
        }
        if (cH == times[0].hours) {
            if (cM > times[0].minutes) {
                return false;
            }
        }
        return true;
    }
    SetHoursMins(branch) {
        let tString = branch.closingTime.toString();
        let h = parseInt(tString.substring(0, 2));
        let m = parseInt(tString.substring(3, 5));
        let times = [];
        let tC = {};
        tC.hours = h;
        tC.minutes = m;
        times.push(tC);
        tString = branch.openingTime.toString();
        h = parseInt(tString.substring(0, 2));
        m = parseInt(tString.substring(3, 5));
        let tO = {};
        tO.hours = h;
        tO.minutes = m;
        times.push(tO);
        return times;
    }
    showOptions() {
        localStorage.setItem('userPhoneNumber', this.model.phoneNumber.toString());
        window.location.reload();
    }
    updateOrderView(item, quantity, userInput) {
        if (this.getOrders() != null) {
            this.tempOrderItems = this.getOrders(); //update list to latest values
        }
        //models should add 'origin' for orderItem in POS
        let orderItem = {
            name: '',
            description: '',
            reference: '',
            price: '',
            weight: '',
            fufilled: false,
            purchased: false,
            preparable: false,
            waitingForPayment: false,
            quantity: 0,
            orderNumber: '',
            phoneNumber: '',
            paymentMethod: '',
            category: '',
            prepTime: 0,
            id_O: '',
            flavour: '',
            meatTemperature: '',
            sauces: [],
            subCategory: ''
        };
        orderItem.quantity = quantity;
        orderItem.description = item.description;
        orderItem.fufilled = false;
        orderItem.name = item.name;
        orderItem.category = item.category;
        orderItem.prepTime = parseInt(item.prepTime);
        orderItem.id_O = (new Date()).toString();
        orderItem.flavour = item.selectedFlavour;
        orderItem.meatTemperature = item.selectedMeatTemperature;
        if (item.subCategory != 'Platter') {
            orderItem.sauces.push(item.selectedSauces.toString());
        }
        if (item.subCategory == 'Platter') {
            orderItem.sauces = item.sauces;
        }
        orderItem.subCategory = item.subCategory;
        if (item.category == 'Meat') {
            if (item.price == '0.00') {
                orderItem.price = (userInput * quantity).toString();
            }
            else {
                orderItem.price = (parseFloat(item.price) * quantity).toString();
            }
        }
        if (item.category != 'Meat') {
            orderItem.price = (parseFloat(item.price) * quantity).toString();
        }
        orderItem.purchased = false;
        orderItem.reference = this.referenceService.currentreference();
        let weight = item.rate * userInput;
        orderItem.weight = (weight.toFixed(2)).toString() + ' grams';
        console.log((weight.toFixed(2)).toString() + ' grams');
        if (item.weight == '') {
            item.weight = null;
        }
        if (item.weight != null) {
            orderItem.weight = parseFloat(item.weight).toFixed(2) + ' grams';
        }
        if (orderItem.weight == '0 grams') {
            orderItem.weight = '-';
        }
        this.tempOrderItems.push(orderItem);
        localStorage.setItem('ordered', JSON.stringify(this.tempOrderItems));
        this.reload();
    }
    calculateTotal() {
        if (this.block == 0) {
            this.orderItems = this.getOrders();
            if (this.orderItems != null) {
                for (var i = 0; i < this.orderItems.length; i++) {
                    this.total += parseFloat(this.orderItems[i].price);
                }
            }
            this.total = Math.round((this.total + Number.EPSILON) * 100) / 100;
            console.log(this.orderItems);
            let num = this.total;
            let n = num.toFixed(2);
            this.totalDisplay = n;
            this.totalUsd = this.total * 0.0906750;
            this.totalUsd = Math.round((this.totalUsd + Number.EPSILON) * 100) / 100;
            //Stripe Button
            this.block = 1;
        }
    }
    back() {
        this.router.navigateByUrl('/menu/' + this.referenceService.currentBranch() + '_' + this.referenceService.currentreference());
    }
    removeItem(item) {
        this.orderItems = this.getOrders();
        let filteredItems = this.orderItems.filter(element => element.id_O != item.id_O);
        console.log(filteredItems);
        this.orderItems = filteredItems;
        localStorage.setItem('ordered', JSON.stringify(this.orderItems));
        this.reload();
    }
    reload() {
        //window.location.reload();//look for better reload method
        this.block = 0;
        this.total = 0;
        this.calculateTotal();
        this.ngOnInit();
    }
    successfulPurchase() {
        let orders = this.getOrders();
        for (let i = 0; i < orders.length; i++) {
            orders[i].phoneNumber = this.model.phoneNumber.toString();
        }
        this.orderService.paidOnlineForOrder(this.getOrders());
        this.router.navigateByUrl('thankyou');
    }
    getOrders() {
        return JSON.parse(localStorage.getItem('ordered'));
    }
    confirmOrder() {
        localStorage.setItem('userPhoneNumber', this.model.phoneNumber.toString());
        this.router.navigateByUrl('checkout');
    }
    payAtTill() {
        let orders = this.getOrders();
        for (let i = 0; i < orders.length; i++) {
            orders[i].phoneNumber = this.model.phoneNumber.toString();
        }
        console.log(orders);
        this.orderService.payAtTill(orders);
    }
    isTablet() {
        if (this.referenceService.currentreference() == 'tablet') {
            return true;
        }
        return false;
    }
    isExternalCustomer() {
        if (this.referenceService.currentreference() == 'clientE') {
            return true;
        }
        return false;
    }
    formatAmount(amount) {
        return parseFloat(amount.split(',').join('')).toLocaleString('en-US', { minimumFractionDigits: 2 });
        ;
    }
    call(index) {
        window.open('tel:' + this.branchPhoneNumbers[index]);
    }
    leave() {
        this.thanks = false;
        this.router.navigateByUrl('order');
    }
};
__decorate([
    Input()
], OrderComponent.prototype, "showPaymentOptions", void 0);
__decorate([
    Input()
], OrderComponent.prototype, "thanks", void 0);
OrderComponent = __decorate([
    Component({
        selector: 'app-order',
        templateUrl: './order.component.html',
        styleUrls: ['./order.component.css']
    })
], OrderComponent);
export { OrderComponent };
//# sourceMappingURL=order.component.js.map