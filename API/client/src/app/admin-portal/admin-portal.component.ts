import { Component, OnInit } from '@angular/core';
import html2canvas from 'html2canvas';
import jspdf from 'jspdf';
import { BusyService } from '../_services/busy.service';
import { DashService } from '../_services/dash.service';
import { ReferenceService } from '../_services/reference.service';

@Component({
  selector: 'app-admin-portal',
  templateUrl: './admin-portal.component.html',
  styleUrls: ['./admin-portal.component.css']
})
export class AdminPortalComponent implements OnInit {

  model: any = {};
  model1: any = {};
  model2: any = {};
  model3: any = {};
  model4: any = {};

  showGraphs = false;

  showDash = true;
  showTotal = false;
  showDetailedTotal = false;
  showRevenue = false;
  showInvoice = false;
  showSettings = false;
  showingReports: Boolean;
  path: any = " ";

  totalSales: any[] = [];
  detailedtotalSales: any[] = [];

  revenue: string;
  payments: any[] = [];
  salesVolume: any = {};
  salesRevenue: any = {};
  salesAverage: any = {};
  revenueAverage: any = {};
  itemsAverage: any = {};
  orderSource: any = {};
  invoices: any = {};
  allSalesRevenue: any = {};

  sidebarButtons:any[] = [];


  constructor(private dashService: DashService, private referenceService: ReferenceService,private busyService:BusyService) { }

  ngOnInit(): void {
    this.populateDashboard();
    var elements = document.getElementsByClassName("sidebar-list-item");
    for (var i = 0, len = elements.length; i < len; i++) {
      this.sidebarButtons.push(elements[i]);
    }
  }

  toggleSideBarActive(index: number){
    this.sidebarButtons.forEach(element => {
      element.classList.remove('active');     
    });

    console.log(this.sidebarButtons);

    this.sidebarButtons[index].classList.toggle('active');
  }

  showTotalReport(){
    this.showingReports = true;
    this.showDash = false;
    this.showTotal = true;
    this.showDetailedTotal = false;
    this.showRevenue = false;
    this.showSettings = false;
    this.showInvoice = false;
    this.toggleSideBarActive(1);
  }
  showDetailedTotalReport(){
    this.showingReports = true;
    this.showDash = false;
    this.showTotal = false;
    this.showDetailedTotal = true;
    this.showRevenue = false;
    this.showSettings = false;
    this.showInvoice = false;
    this.toggleSideBarActive(2);
  }
  showRevenueReport(){
    this.showingReports = true;
    this.showDash = false;
    this.showTotal = false;
    this.showDetailedTotal = false;
    this.showRevenue = true;
    this.showSettings = false;
    this.showInvoice = false;
    this.toggleSideBarActive(3);
  }
  showInvoiceReport(){
    this.showingReports = true;
    this.showDash = false;
    this.showTotal = false;
    this.showDetailedTotal = false;
    this.showSettings = false;
    this.showRevenue = false;
    this.showInvoice = true;
    this.toggleSideBarActive(4);
  }
  showSettingsPage(){
    this.showingReports = true;
    this.showDash = false;
    this.showTotal = false;
    this.showDetailedTotal = false;
    this.showRevenue = false;
    this.showInvoice = false;
    this.showSettings = true;
    this.toggleSideBarActive(6);
  }
  showDashboard(){
    this.showingReports = false;
    this.showDash = true;
    this.showTotal = false;
    this.showDetailedTotal = false;
    this.showRevenue = false;
    this.showSettings = false;
    this.showInvoice = false;
    this.toggleSideBarActive(0);
  }

  reportDto(entity: any): any 
  {
    entity.branchId = this.referenceService.currentBranch();

    return entity;
  }

  populateDashboard(){
    this.dashService.salesVolume(this.referenceService.currentBranch()).subscribe(
      response =>{
        console.log(response);
        this.salesVolume = response;
      },
      error =>{
        console.log(error);
      }
    )

    this.dashService.salesRevenue(this.referenceService.currentBranch()).subscribe(
      response =>{
        console.log(response);
        this.salesRevenue = response;
      },
      error =>{
        console.log(error);
      }
    )
    this.dashService.allSalesRevenue().subscribe(
      response =>{
        console.log(response);
        this.allSalesRevenue = response;
      },
      error =>{
        console.log(error);
      }
    )
    this.dashService.salesAverage(this.referenceService.currentBranch()).subscribe(
      response =>{
        console.log(response);
        this.salesAverage = response;
      },
      error =>{
        console.log(error);
      }
    )    
    this.dashService.revenueAverage(this.referenceService.currentBranch()).subscribe(
      response =>{
        console.log(response);
        this.revenueAverage = response;
      },
      error =>{
        console.log(error);
      }
    )
    this.dashService.itemsAverage(this.referenceService.currentBranch()).subscribe(
      response =>{
        console.log(response);
        this.itemsAverage = response;
      },
      error =>{
        console.log(error);
      }
    )
    this.dashService.orderSource(this.referenceService.currentBranch()).subscribe(
      response =>{
        console.log(response);
        this.orderSource = response;
      },
      error =>{
        console.log(error);
      }
    )
  }

  generateReport(type: string){
    if(type == 'total'){
      this.model.searched = true;

      this.dashService.totalSales(this.reportDto(this.model)).subscribe(
        response => {
          this.totalSales = response;

          if(response.length == 0){
            this.model.empty = true;
          }else{
            this.model.empty = false;
          }
        }
      )
    }
    this.busyService.idle();

    if(type == 'detailedtotal'){
      this.model1.searched = true;

      this.dashService.totalDetailedSales(this.reportDto(this.model1)).subscribe(
        response => {
          this.detailedtotalSales = response;

          if(response.length == 0){
            this.model1.empty = true;
          }else{
            this.model1.empty = false;
          }
          
        }
      )
    }

    if(type == 'revenue'){
      this.dashService.revenue(this.reportDto(this.model2)).subscribe(
        response => {
          this.revenue = response.orderRevenue;
        }
      )
    }

    if(type == 'payment'){
      this.dashService.payment(this.reportDto(this.model3)).subscribe(
        response => {
          this.payments = response;
        }
      )
    }

    if(type == 'invoice'){
      this.model4.searched = true;
      this.dashService.invoice(this.reportDto(this.model4)).subscribe(
        response => {
          this.invoices = response;

          if(response.length == 0){
            this.model4.empty = true;
          }else{
            this.model4.empty = false;
          }
        }
      )
    }

  }
  getTotal(item: any, origin: string){
    if(origin == "total"){
      let values = item;
      let total: number = 0;
      values.forEach(element => {
        total = total + parseFloat(element.orderRevenue.split(',').join(''));      
      });

      let tot = parseFloat(total.toFixed(2));
      return tot.toLocaleString('en-US', {minimumFractionDigits: 2});
    }  
    if(origin == "dtotal"){
      let values = item;
      let total: number = 0;
      let quantity: number = 0;
      let weight: number = 0;
      
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

      let result: any[] = [];

      result.push(quantity.toString());

      result.push(weight.toLocaleString('en-US', {minimumFractionDigits: 2}));

      result.push(tot.toLocaleString('en-US', {minimumFractionDigits: 2}));

      return result;
    }  
    if(origin == "payment"){
      let values = item;
      let total: number = 0;
      
      values.forEach(element => {
        total = total + parseFloat(element.amount.split(',').join(''));      
      });

      let tot = parseFloat(total.toFixed(2));

      return tot.toLocaleString('en-US', {minimumFractionDigits: 2});
    }

    if(origin == "invoice"){
      let values = item;
      let total: number = 0;
      
      values.forEach(element => {
        total = total + parseFloat(element.price.split(',').join(''));      
      });

      let tot = parseFloat(total.toFixed(2));

      return tot.toLocaleString('en-US', {minimumFractionDigits: 2});
    }
  }

  expandToggle(item: any){
    item.extra = !item.extra;
  }

  exportToExcel(){    
    this.dashService.exportToExcel(this.referenceService.currentBranch());
  }

  exportDetailReportToExcel(){
    this.dashService.exportDetailReportToExcel(this.model1);
  }

  exportTotalReportToExcel(){
    this.dashService.exportTotalReportToExcel(this.model);
  }
  showSpin = false;
  
  public convertToPDF()
  {
    this.showSpin = true;
   var data = document.getElementById('contentToConvert');
   html2canvas(data).then(canvas => {

   // Few necessary setting options
   var imgWidth = 208;
   var imgHeight = canvas.height * imgWidth / canvas.width;

   console.log(imgHeight);
   console.log(imgWidth);
   
   const contentDataURL = canvas.toDataURL('image/png')
   let pdf = new jspdf('p', 'mm', 'a4'); // A4 size page of PDF
   var position = 0;
   pdf.addImage(contentDataURL, 'JPEG', 0, position, imgWidth, imgHeight)
   pdf.save('rodizio-express-report-' + new Date() + '.pdf'); // Generated PDF
   this.showSpin = false;
  });
  }

  ToggleSideBar(){
    var sidebar = document.getElementById('sidebar');
    sidebar.classList.toggle('open');

    var content = document.getElementById('content');
    content.classList.toggle('open');
  }

}
