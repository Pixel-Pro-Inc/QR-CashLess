import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ElementRef ,ViewChild } from '@angular/core';
import { Local } from 'protractor/built/driverProviders';
import { map } from 'rxjs/operators';
import { AdminComponent } from '../admin/admin.component';
import { OrderItem } from '../_models/orderItem';
import { AccountService } from '../_services/account.service';
import { ReferenceService } from '../_services/reference.service';
import jspdf from 'jspdf';
import html2canvas from 'html2canvas';
import { Router } from '@angular/router';

@Component({
  selector: 'app-receipt',
  templateUrl: './receipt.component.html',
  styleUrls: ['./receipt.component.css']
})
export class ReceiptComponent implements OnInit {
  orders: OrderItem[]
  orderItems: OrderItem[] = [];
  baseUrl: string;
  invoiceNum: string;
  date: any;
  total = 0;
  title = 'html-to-pdf-angular-application';
  private adminComponent: AdminComponent;

  constructor(private http: HttpClient, private accountService: AccountService, private referenceService: ReferenceService, private router: Router) { }

  ngOnInit(): void {
    this.baseUrl = this.accountService.baseUrl;
    this.orders = JSON.parse(localStorage.getItem('ordered'));

    this.invoiceNum = this.orders[0].orderNumber;


    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    let _today = mm + '/' + dd + '/' + yyyy;

    this.date = _today;

    //Clear Orders List
    let empty: OrderItem[] = [];
    localStorage.setItem('ordered', JSON.stringify(empty));

    //this.adminComponent.pushreceipt(this);
  }
  Paid(): boolean{
    if(this.orders[0].purchased){
      return true;
    }

    return false;
  }
  getOrderNum():string{
    let x: string = this.orders[0].orderNumber;
    let indexOfBreak = x.indexOf('_');

    return x.slice(indexOfBreak + 1, x.length);;
  }
  getDate(): string{
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();
    
    let _today = mm + '/' + dd + '/' + yyyy;
    return _today;
  }
  public convertToPDF()
  {
   var data = document.getElementById('contentToConvert');
   html2canvas(data).then(canvas => {
   // Few necessary setting options
   var imgWidth = 208;
   var pageHeight = 295;
   var imgHeight = canvas.height * imgWidth / canvas.width;
   var heightLeft = imgHeight;
   
   const contentDataURL = canvas.toDataURL('image/png')
   let pdf = new jspdf('p', 'mm', 'a4'); // A4 size page of PDF
   var position = 0;
   pdf.addImage(contentDataURL, 'PNG', 0, position, imgWidth, imgHeight)
   pdf.save('rodizio-express-receipt.pdf'); // Generated PDF
  });
  }
  getTotal(item: any, origin: string){
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
  makeNewOrder(){
    this.router.navigateByUrl('/menu/' + this.referenceService.currentBranch() + '_' + this.referenceService.currentreference());
  }
  formatAmount(amount: string){
    return parseFloat(amount.split(',').join('')).toLocaleString('en-US', {minimumFractionDigits: 2});
  }
}
