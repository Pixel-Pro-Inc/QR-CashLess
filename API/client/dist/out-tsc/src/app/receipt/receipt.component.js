import { __decorate } from "tslib";
import { Component } from '@angular/core';
import jspdf from 'jspdf';
import html2canvas from 'html2canvas';
let ReceiptComponent = class ReceiptComponent {
    constructor(http, accountService, referenceService, router) {
        this.http = http;
        this.accountService = accountService;
        this.referenceService = referenceService;
        this.router = router;
        this.showSpin = false;
        this.orderItems = [];
        this.total = 0;
        this.title = 'html-to-pdf-angular-application';
        this.orderTime = '';
    }
    ngOnInit() {
        this.orderTime = new Date().toLocaleTimeString();
        this.baseUrl = this.accountService.baseUrl;
        this.orders = JSON.parse(localStorage.getItem('ordered'));
        this.invoiceNum = this.orders[0].orderNumber;
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0');
        var yyyy = today.getFullYear();
        let _today = mm + '/' + dd + '/' + yyyy;
        this.date = _today;
        //Clear Orders List
        let empty = [];
        localStorage.setItem('ordered', JSON.stringify(empty));
    }
    GetCurrentTime() {
        return this.orderTime;
    }
    GetPrepTime() {
        let pTime = 0;
        for (let i = 0; i < this.orders.length; i++) {
            if (pTime < this.orders[i].prepTime) {
                pTime = this.orders[i].prepTime;
            }
        }
        return pTime;
    }
    Paid() {
        if (this.orders[0].purchased) {
            return true;
        }
        return false;
    }
    getOrderNum() {
        let x = this.orders[0].orderNumber;
        let indexOfBreak = x.indexOf('_');
        return x.slice(indexOfBreak + 1, x.length);
        ;
    }
    getDate() {
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
        var yyyy = today.getFullYear();
        let _today = mm + '/' + dd + '/' + yyyy;
        return _today;
    }
    convertToPDF() {
        this.showSpin = true;
        var data = document.getElementById('contentToConvert');
        html2canvas(data).then(canvas => {
            // Few necessary setting options
            var imgWidth = canvas.width * .1; //208;
            var imgHeight = canvas.height * .1; // * imgWidth / canvas.width;
            console.log(imgHeight);
            console.log(imgWidth);
            const contentDataURL = canvas.toDataURL('image/png');
            let pdf = new jspdf('p', 'mm', 'a4'); // A4 size page of PDF
            var position = 0;
            pdf.addImage(contentDataURL, 'JPEG', 0, position, imgWidth, imgHeight);
            pdf.save('rodizio-express-receipt-' + this.getOrderNum() + '.pdf'); // Generated PDF
            this.showSpin = false;
        });
    }
    getTotal(item, origin) {
        if (origin == "invoice") {
            let values = item;
            let total = 0;
            values.forEach(element => {
                total = total + parseFloat(element.price.split(',').join(''));
            });
            let tot = parseFloat(total.toFixed(2));
            return tot.toLocaleString('en-US', { minimumFractionDigits: 2 });
        }
    }
    getName(order) {
        /*let str = '';
        for (let i = 0; i < order.name.length; i++) {
          if(order.name[i] == ' ' && order.name[i + 1] == 'x'){
            break;
          }
    
          str += order.name[i];
        }*/
        return order.name; // str;
    }
    getUnitPrice(order) {
        let amount = (parseFloat(order.price) / order.quantity).toString();
        return parseFloat(amount.split(',').join('')).toLocaleString('en-US', { minimumFractionDigits: 2 });
    }
    makeNewOrder() {
        this.router.navigateByUrl('/menu/' + this.referenceService.currentBranch() + '_' + this.referenceService.currentreference());
    }
    formatAmount(amount) {
        return parseFloat(amount.split(',').join('')).toLocaleString('en-US', { minimumFractionDigits: 2 });
    }
};
ReceiptComponent = __decorate([
    Component({
        selector: 'app-receipt',
        templateUrl: './receipt.component.html',
        styleUrls: ['./receipt.component.css']
    })
], ReceiptComponent);
export { ReceiptComponent };
//# sourceMappingURL=receipt.component.js.map