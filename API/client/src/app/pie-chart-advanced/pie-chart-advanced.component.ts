import { Component, Input, OnInit } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NgxChartsModule } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-pie-chart-advanced',
  templateUrl: './pie-chart-advanced.component.html',
  styleUrls: ['./pie-chart-advanced.component.css']
})
export class PieChartAdvancedComponent{

  @Input() single: any[] = [];
  
  view: any[] = [600, 400];

  // options
  gradient: boolean = false;
  showLegend: boolean = true;
  showLabels: boolean = true;
  isDoughnut: boolean = true;

  colorScheme = {
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
  };

  onSelect(data): void {
    console.log('Item clicked', JSON.parse(JSON.stringify(data)));
  }

  onActivate(data): void {
    console.log('Activate', JSON.parse(JSON.stringify(data)));
  }

  onDeactivate(data): void {
    console.log('Deactivate', JSON.parse(JSON.stringify(data)));
  }
}
