import { Component, OnInit } from '@angular/core';
import { BusyService } from '../_services/busy.service';
import { DashService } from '../_services/dash.service';
import { ReferenceService } from '../_services/reference.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  model: any = {};
  model1: any = {};
  model2: any = {};
  model3: any = {};
  model4: any = {};

  showDash = true;
  showTotal = false;
  showDetailedTotal = false;
  showRevenue = false;
  showPayment = false;
  showInvoice = false;
  showingReports: Boolean;
  path: any = " ";

  totalSales: any[] = [];
  detailedtotalSales: any[] = [];

  revenue: string;
  payments: any[] = [];
  salesVolume: any = {};
  salesRevenue: any = {};
  invoices: any = {};
  allSalesRevenue: any = {};


  constructor(private dashService: DashService, private referenceService: ReferenceService,private busyService:BusyService) { }

  ngOnInit(): void {
    this.populateDashboard();
  }

  showTotalReport(){
    this.showingReports = true;
    this.showDash = false;
    this.showTotal = true;
    this.showDetailedTotal = false;
    this.showRevenue = false;
    this.showPayment = false;
    this.showInvoice = false;
  }
  showDetailedTotalReport(){
    this.showingReports = true;
    this.showDash = false;
    this.showTotal = false;
    this.showDetailedTotal = true;
    this.showRevenue = false;
    this.showPayment = false;
    this.showInvoice = false;
  }
  showRevenueReport(){
    this.showingReports = true;
    this.showDash = false;
    this.showTotal = false;
    this.showDetailedTotal = false;
    this.showRevenue = true;
    this.showPayment = false;
    this.showInvoice = false;
  }
  showPaymentReport(){
    this.showingReports = true;
    this.showDash = false;
    this.showTotal = false;
    this.showDetailedTotal = false;
    this.showRevenue = false;
    this.showPayment = true;
    this.showInvoice = false;
  }
  showInvoiceReport(){
    this.showingReports = true;
    this.showDash = false;
    this.showTotal = false;
    this.showDetailedTotal = false;
    this.showRevenue = false;
    this.showPayment = false;
    this.showInvoice = true;
  }
  showDashboard(){
    this.showingReports = false;
    this.showDash = true;
    this.showTotal = false;
    this.showDetailedTotal = false;
    this.showRevenue = false;
    this.showPayment = false;
    this.showInvoice = false;
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
  detailReportToExcel(){
    // NOTE: The reason why it wasn't working is because we weren't subscribed to it.
    // The real mystery is why it choose to fire another logic instead
    this.dashService.detailReportToExcel(this.reportDto(this.model1)).subscribe(
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

}
