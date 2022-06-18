import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { BaseServiceService } from './-base-service.service';
let DashService = class DashService extends BaseServiceService {
    constructor(http, toastr, busyService) {
        super(http);
        this.toastr = toastr;
        this.busyService = busyService;
    }
    // REFACTOR: The below three methods could be refactored. We can't have repeating code for such important business logic
    totalSales(model) {
        this.busyService.busy_1();
        return this.http.post(this.baseUrl + 'report/sales/total', model).pipe(map((response) => {
            this.busyService.idle_1();
            return response;
        }));
    }
    totalDetailedSales(model) {
        this.busyService.busy_1();
        return this.http.post(this.baseUrl + 'report/sales/item', model).pipe(map((response) => {
            this.busyService.idle_1();
            return response;
        }));
    }
    revenue(model) {
        this.busyService.busy_1();
        return this.http.post(this.baseUrl + 'report/sales/summary', model).pipe(map((response) => {
            this.busyService.idle_1();
            return response;
        }));
    }
    payment(model) {
        this.busyService.busy_1();
        return this.http.post(this.baseUrl + 'report/sales/paymentmethods', model).pipe(map((response) => {
            this.busyService.idle_1();
            return response;
        }));
    }
    salesVolume(branchId) {
        return this.http.get(this.baseUrl + 'report/sales/thismonth/volume/' + branchId).pipe(map((response) => {
            return response;
        }));
    }
    salesRevenue(branchId) {
        return this.http.get(this.baseUrl + 'report/sales/thismonth/revenue/' + branchId).pipe(map((response) => {
            return response;
        }));
    }
    allSalesRevenue() {
        return this.http.get(this.baseUrl + 'report/sales/thismonth/allrevenue').pipe(map((response) => {
            return response;
        }));
    }
    salesAverage(branchId) {
        return this.http.get(this.baseUrl + 'report/sales/thismonth/averagevolume/' + branchId).pipe(map((response) => {
            return response;
        }));
    }
    revenueAverage(branchId) {
        return this.http.get(this.baseUrl + 'report/sales/thismonth/averagerevenue/' + branchId).pipe(map((response) => {
            return response;
        }));
    }
    itemsAverage(branchId) {
        return this.http.get(this.baseUrl + 'report/sales/thismonth/averageitems/' + branchId).pipe(map((response) => {
            return response;
        }));
    }
    orderSource(branchId) {
        return this.http.get(this.baseUrl + 'report/sales/thismonth/ordersource/' + branchId).pipe(map((response) => {
            return response;
        }));
    }
    invoice(model) {
        this.busyService.busy_1();
        return this.http.post(this.baseUrl + 'report/sales/invoice', model).pipe(map((response) => {
            this.busyService.idle_1();
            return response;
        }));
    }
    exportToExcel(branchId) {
        window.open(this.baseUrl + 'excel/export/' + branchId);
    }
    exportDetailReportToExcel(model) {
        this.http.post(this.baseUrl + 'report/excel/export-detailedsales', model, { responseType: 'blob' }).subscribe((response) => {
            let dataType = response.type;
            let binaryData = [];
            binaryData.push(response);
            let downloadLink = document.createElement('a');
            downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, { type: dataType }));
            downloadLink.setAttribute('download', 'Rodizio Express Data_Export ' + (new Date()).toString());
            document.body.appendChild(downloadLink);
            downloadLink.click();
        });
    }
    exportTotalReportToExcel(model) {
        this.http.post(this.baseUrl + 'report/excel/export-totalsales', model, { responseType: 'blob' }).subscribe((response) => {
            let dataType = response.type;
            let binaryData = [];
            binaryData.push(response);
            let downloadLink = document.createElement('a');
            downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, { type: dataType }));
            downloadLink.setAttribute('download', 'Rodizio Express Data_Export ' + (new Date()).toString());
            document.body.appendChild(downloadLink);
            downloadLink.click();
        });
    }
};
DashService = __decorate([
    Injectable({
        providedIn: 'root'
    })
], DashService);
export { DashService };
//# sourceMappingURL=dash.service.js.map