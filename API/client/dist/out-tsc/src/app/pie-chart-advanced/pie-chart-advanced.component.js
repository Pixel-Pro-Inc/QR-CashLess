import { __decorate } from "tslib";
import { Component, Input } from '@angular/core';
let PieChartAdvancedComponent = class PieChartAdvancedComponent {
    constructor() {
        /*single: any[] = [
          {
            "name" : "Walk in",
            "value" : 0
          },
          {
            "name" : "Call in",
            "value" : 0
          },
          {
            "name" : "Online",
            "value" : 0
          },
          {
            "name" : "Delivery",
            "value" : 0
          }
        ];*/
        this.view = [600, 400];
        // options
        this.gradient = false;
        this.showLegend = true;
        this.showLabels = true;
        this.isDoughnut = true;
        this.colorScheme = {
            domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
        };
    }
    sleep(ms) {
        return new Promise((resolve) => {
            setTimeout(resolve, ms);
        });
    }
    onSelect(data) {
        console.log('Item clicked', JSON.parse(JSON.stringify(data)));
    }
    onActivate(data) {
        console.log('Activate', JSON.parse(JSON.stringify(data)));
    }
    onDeactivate(data) {
        console.log('Deactivate', JSON.parse(JSON.stringify(data)));
    }
};
__decorate([
    Input()
], PieChartAdvancedComponent.prototype, "single", void 0);
PieChartAdvancedComponent = __decorate([
    Component({
        selector: 'app-pie-chart-advanced',
        templateUrl: './pie-chart-advanced.component.html',
        styleUrls: ['./pie-chart-advanced.component.css']
    })
], PieChartAdvancedComponent);
export { PieChartAdvancedComponent };
//# sourceMappingURL=pie-chart-advanced.component.js.map