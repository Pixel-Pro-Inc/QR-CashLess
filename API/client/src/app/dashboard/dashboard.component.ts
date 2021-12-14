import { Component, OnInit } from '@angular/core';
import { DashService } from '../_services/dash.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  totalSalesModel: any = {};
  showTotal = false;
  showingReports: Boolean;


  constructor(private dashService: DashService) { }

  ngOnInit(): void {
  }

  showTotalReport(){
    this.showingReports = true;
    this.showTotal = true;
  }

  generateReport(type: string){
    if(type == 'total'){
      this.dashService.totalSales(this.totalSalesModel);
    }

  }

}
