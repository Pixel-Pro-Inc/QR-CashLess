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
  path: any = " ";

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
  exportToExcel() {
    //path= Window.path
    this.dashService.exportexceldata();
  }

  //I havent seen how or why to do this, but I put it here just in case
  importToExcel(model: any, filename: any) {
    this.dashService.importexceldata(model, filename);
  }

}
