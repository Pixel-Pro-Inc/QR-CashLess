import { Component, OnInit } from '@angular/core';
import { BusyService } from '../_services/busy.service';
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
  path: any = " ";

  constructor(private dashService: DashService, private busyService: BusyService) { }

  ngOnInit(): void {
  }

  showTotalReport(){
    this.showingReports = true;
    this.showTotal = true;
  }

  generateReport(type: string) {
    this.busyService.busy();
    if(type == 'total'){
      this.dashService.totalSales(this.totalSalesModel);
    }
    this.busyService.idle();

  }
  exportToExcel() {
    //path= Window.path
    this.dashService.exportexceldata();
  }

  //I havent seen how or why to do this, but I put it here just in case
  importToExcel(model: any, filename: any) {
    this.dashService.importexceldata(model, filename);
  }

}
