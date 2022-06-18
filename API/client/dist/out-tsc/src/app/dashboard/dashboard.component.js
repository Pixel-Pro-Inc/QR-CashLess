import { __decorate } from "tslib";
import { Component } from '@angular/core';
import html2canvas from 'html2canvas';
import jspdf from 'jspdf';
let DashboardComponent = class DashboardComponent {
    constructor(dashService, referenceService, busyService) {
        this.dashService = dashService;
        this.referenceService = referenceService;
        this.busyService = busyService;
        this.model = {};
        this.model1 = {};
        this.model2 = {};
        this.model3 = {};
        this.model4 = {};
        this.showDash = true;
        this.showTotal = false;
        this.showDetailedTotal = false;
        this.showRevenue = false;
        this.showPayment = false;
        this.showInvoice = false;
        this.path = " ";
        this.totalSales = [];
        this.detailedtotalSales = [];
        this.payments = [];
        this.salesVolume = {};
        this.salesRevenue = {};
        this.invoices = {};
        this.allSalesRevenue = {};
        this.showSpin = false;
    }
    ngOnInit() {
        this.populateDashboard();
    }
    showTotalReport() {
        this.showingReports = true;
        this.showDash = false;
        this.showTotal = true;
        this.showDetailedTotal = false;
        this.showRevenue = false;
        this.showPayment = false;
        this.showInvoice = false;
    }
    showDetailedTotalReport() {
        this.showingReports = true;
        this.showDash = false;
        this.showTotal = false;
        this.showDetailedTotal = true;
        this.showRevenue = false;
        this.showPayment = false;
        this.showInvoice = false;
    }
    showRevenueReport() {
        this.showingReports = true;
        this.showDash = false;
        this.showTotal = false;
        this.showDetailedTotal = false;
        this.showRevenue = true;
        this.showPayment = false;
        this.showInvoice = false;
    }
    showPaymentReport() {
        this.showingReports = true;
        this.showDash = false;
        this.showTotal = false;
        this.showDetailedTotal = false;
        this.showRevenue = false;
        this.showPayment = true;
        this.showInvoice = false;
    }
    showInvoiceReport() {
        this.showingReports = true;
        this.showDash = false;
        this.showTotal = false;
        this.showDetailedTotal = false;
        this.showRevenue = false;
        this.showPayment = false;
        this.showInvoice = true;
    }
    showDashboard() {
        this.showingReports = false;
        this.showDash = true;
        this.showTotal = false;
        this.showDetailedTotal = false;
        this.showRevenue = false;
        this.showPayment = false;
        this.showInvoice = false;
    }
    reportDto(entity) {
        entity.branchId = this.referenceService.currentBranch();
        return entity;
    }
    populateDashboard() {
        this.dashService.salesVolume(this.referenceService.currentBranch()).subscribe(response => {
            console.log(response);
            this.salesVolume = response;
        }, error => {
            console.log(error);
        });
        this.dashService.salesRevenue(this.referenceService.currentBranch()).subscribe(response => {
            console.log(response);
            this.salesRevenue = response;
        }, error => {
            console.log(error);
        });
        this.dashService.allSalesRevenue().subscribe(response => {
            console.log(response);
            this.allSalesRevenue = response;
        }, error => {
            console.log(error);
        });
    }
    generateReport(type) {
        if (type == 'total') {
            this.model.searched = true;
            this.dashService.totalSales(this.reportDto(this.model)).subscribe(response => {
                this.totalSales = response;
                if (response.length == 0) {
                    this.model.empty = true;
                }
                else {
                    this.model.empty = false;
                }
            });
        }
        this.busyService.idle();
        if (type == 'detailedtotal') {
            this.model1.searched = true;
            this.dashService.totalDetailedSales(this.reportDto(this.model1)).subscribe(response => {
                this.detailedtotalSales = response;
                if (response.length == 0) {
                    this.model1.empty = true;
                }
                else {
                    this.model1.empty = false;
                }
            });
        }
        if (type == 'revenue') {
            this.dashService.revenue(this.reportDto(this.model2)).subscribe(response => {
                this.revenue = response.orderRevenue;
            });
        }
        if (type == 'payment') {
            this.dashService.payment(this.reportDto(this.model3)).subscribe(response => {
                this.payments = response;
            });
        }
        if (type == 'invoice') {
            this.model4.searched = true;
            this.dashService.invoice(this.reportDto(this.model4)).subscribe(response => {
                this.invoices = response;
                if (response.length == 0) {
                    this.model4.empty = true;
                }
                else {
                    this.model4.empty = false;
                }
            });
        }
    }
    getTotal(item, origin) {
        if (origin == "total") {
            let values = item;
            let total = 0;
            values.forEach(element => {
                total = total + parseFloat(element.orderRevenue.split(',').join(''));
            });
            let tot = parseFloat(total.toFixed(2));
            return tot.toLocaleString('en-US', { minimumFractionDigits: 2 });
        }
        if (origin == "dtotal") {
            let values = item;
            let total = 0;
            let quantity = 0;
            let weight = 0;
            values.forEach(element => {
                total = total + parseFloat(element.orderRevenue.split(',').join(''));
            });
            values.forEach(element => {
                quantity = quantity + element.quantity;
            });
            values.forEach(element => {
                weight = weight + parseFloat(element.weight.split(',').join(''));
            });
            let tot = parseFloat(total.toFixed(2));
            let result = [];
            result.push(quantity.toString());
            result.push(weight.toLocaleString('en-US', { minimumFractionDigits: 2 }));
            result.push(tot.toLocaleString('en-US', { minimumFractionDigits: 2 }));
            return result;
        }
        if (origin == "payment") {
            let values = item;
            let total = 0;
            values.forEach(element => {
                total = total + parseFloat(element.amount.split(',').join(''));
            });
            let tot = parseFloat(total.toFixed(2));
            return tot.toLocaleString('en-US', { minimumFractionDigits: 2 });
        }
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
    expandToggle(item) {
        item.extra = !item.extra;
    }
    exportToExcel() {
        this.dashService.exportToExcel(this.referenceService.currentBranch());
    }
    exportDetailReportToExcel() {
        this.dashService.exportDetailReportToExcel(this.model1);
    }
    exportTotalReportToExcel() {
        this.dashService.exportTotalReportToExcel(this.model);
    }
    convertToPDF() {
        this.showSpin = true;
        var data = document.getElementById('contentToConvert');
        html2canvas(data).then(canvas => {
            // Few necessary setting options
            var imgWidth = 208;
            var imgHeight = canvas.height * imgWidth / canvas.width;
            console.log(imgHeight);
            console.log(imgWidth);
            const contentDataURL = canvas.toDataURL('image/png');
            let pdf = new jspdf('p', 'mm', 'a4'); // A4 size page of PDF
            var position = 0;
            pdf.addImage(contentDataURL, 'JPEG', 0, position, imgWidth, imgHeight);
            pdf.save('rodizio-express-report-' + new Date() + '.pdf'); // Generated PDF
            this.showSpin = false;
        });
    }
};
DashboardComponent = __decorate([
    Component({
        selector: 'app-dashboard',
        templateUrl: './dashboard.component.html',
        styleUrls: ['./dashboard.component.css']
    })
], DashboardComponent);
export { DashboardComponent };
//# sourceMappingURL=dashboard.component.js.map