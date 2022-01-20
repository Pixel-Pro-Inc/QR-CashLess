import { HttpClient } from '@angular/common/http';
import { Component, OnInit} from '@angular/core';
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
  showSpin = false;
  orders: OrderItem[]
  orderItems: OrderItem[] = [];
  baseUrl: string;
  invoiceNum: string;
  date: any;
  total = 0;
  title = 'html-to-pdf-angular-application';
  orderTime: string = '';

  constructor(private http: HttpClient, private accountService: AccountService, private referenceService: ReferenceService, private router: Router) { }

  ngOnInit(): void {
    this.orderTime = new Date().toLocaleTimeString();
    this.baseUrl = this.accountService.baseUrl;
    this.orders = JSON.parse(localStorage.getItem('ordered'));

    this.invoiceNum = this.orders[0].orderNumber;


    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();

    let _today = mm + '/' + dd + '/' + yyyy;

    this.date = _today;

    //Clear Orders List
    let empty: OrderItem[] = [];
    localStorage.setItem('ordered', JSON.stringify(empty));
  }
  GetCurrentTime(){
    return this.orderTime;
  }
  GetPrepTime(){
    let pTime = 0;

    for (let i = 0; i < this.orders.length; i++) {
      if(pTime < this.orders[i].prepTime){
        pTime = this.orders[i].prepTime;
      }
    }

    return pTime;
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
    this.showSpin = true;
   var data = document.getElementById('contentToConvert');
   html2canvas(data).then(canvas => {

   // Few necessary setting options
   var imgWidth = canvas.width *.1;//208;
   var imgHeight = canvas.height *.1;// * imgWidth / canvas.width;

   console.log(imgHeight);
   console.log(imgWidth);
   
   const contentDataURL = canvas.toDataURL('image/png')
   let pdf = new jspdf('p', 'mm', 'a4'); // A4 size page of PDF
   var position = 0;
   pdf.addImage(contentDataURL, 'JPEG', 0, position, imgWidth, imgHeight)
   pdf.save('rodizio-express-receipt-' + this.getOrderNum() + '.pdf'); // Generated PDF
   this.showSpin = false;
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
