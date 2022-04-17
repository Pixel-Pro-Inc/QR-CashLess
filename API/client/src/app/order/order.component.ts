import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { map } from 'rxjs/operators';
import { OrderItem } from '../_models/orderItem';
import { AccountService } from '../_services/account.service';
import { ReferenceService } from '../_services/reference.service';
import { error } from '@angular/compiler/src/util';
import { NavigationEnd, Router } from '@angular/router';
import { OrderService } from '../_services/order.service';
import { MenuItem } from '../_models/menuItem';
import { Branch } from '../_models/branch';
import { BranchService } from '../_services/branch.service';
import { float } from 'html2canvas/dist/types/css/property-descriptors/float';
import { element } from 'protractor';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  @Input() showPaymentOptions: boolean;
  @Input() thanks: boolean;

  total = 0.00;
  totalDisplay = '';
  totalUsd = 0.00;

  
  orderItems : OrderItem[];
  tempOrderItems : OrderItem[] = [];

  model: any = {};

  block = 0;

  onlineOrderAvailable: Boolean;
  branchPhoneNumbers: number[];

  constructor(private router: Router, private orderService: OrderService, private referenceService: ReferenceService, private branchService: BranchService) { }

  ngOnInit(): void {
    this.model.phoneNumber = JSON.parse(localStorage.getItem('userPhoneNumber'));

    this.orderItems = this.getOrders();    
    
    this.branchService.getRestBranches('branch/getbranches').subscribe(response => {
      console.log(response);
      
      let RestBranches: Branch[] = response;

      for (let i = 0; i < RestBranches.length; i++) {
        if(RestBranches[i].id == this.referenceService.currentBranch()){

          console.log(RestBranches[i].phoneNumbers);
          
          this.branchPhoneNumbers = RestBranches[i].phoneNumbers;

          if(RestBranches[i].lastActive < 30 && this.isOpen(RestBranches[i])){
            this.onlineOrderAvailable = true;
          }
        }
      }
      
    });
  }

  isOpen (b: Branch):Boolean{
    let currentTime = new Date();

    let times:any[] = this.SetHoursMins(b);

    let cH = currentTime.getHours();

    if(cH > times[0].hours){
      return false;
    }

    if(cH < times[1].hours){
      return false;
    }

    let cM = currentTime.getMinutes();

    if(cH == times[1].hours){
      if(cM < times[1].minutes){
        return false;
      }
    }

    if(cH == times[0].hours){
      if(cM > times[0].minutes){
        return false;
      }
    }

    return true;
  }

  SetHoursMins(branch: Branch){
    let tString = branch.closingTime.toString();

    let h: number = parseInt(tString.substring(0, 2));

    let m: number = parseInt(tString.substring(3, 5));

    let times: any = [];

    let tC: any = {};

    tC.hours = h;

    tC.minutes = m;

    times.push(tC);

    tString = branch.openingTime.toString();

    h = parseInt(tString.substring(0, 2));

    m = parseInt(tString.substring(3, 5));

    let tO: any = {};

    tO.hours = h;

    tO.minutes = m;

    times.push(tO);

    return times;
  }

  showOptions(){
    localStorage.setItem('userPhoneNumber', this.model.phoneNumber.toString());

    window.location.reload();
  }

  public updateOrderView(item: MenuItem, quantity: number, userInput: number) {
    if(this.getOrders() != null){
      this.tempOrderItems = this.getOrders();//update list to latest values
    }    
    //models should add 'origin' for orderItem in POS
    let orderItem : OrderItem = {
      name: '',
      description: '',
      reference: '',
      price: '',
      weight: '',
      fufilled: false,
      purchased: false,
      preparable: false,
      waitingForPayment: false,
      quantity: 0,
      orderNumber: '',
      phoneNumber: '',
      paymentMethod: '',
      category: '',
      prepTime: 0,
      id_O: '',
      flavour: '',
      meatTemperature: '',
      sauces: [],
      subCategory: ''
    };

    orderItem.quantity = quantity;
    orderItem.description = item.description;
    orderItem.fufilled = false;
    orderItem.name = item.name;
    orderItem.category = item.category;
    orderItem.prepTime = parseInt(item.prepTime);
    orderItem.id_O = (new Date()).toString();
    orderItem.flavour = item.selectedFlavour;
    orderItem.meatTemperature = item.selectedMeatTemperature;

    if(item.subCategory != 'Platter'){
      orderItem.sauces.push(item.selectedSauces.toString());
    }

    if(item.subCategory == 'Platter'){
      orderItem.sauces = item.sauces;
    }
    
    orderItem.subCategory = item.subCategory;

    if(item.category == 'Meat'){
      if(item.price == '0.00'){
        orderItem.price = (userInput * quantity).toString();
      }
      else{
        orderItem.price = (parseFloat(item.price) * quantity).toString();
      }  
    }

    if(item.category != 'Meat'){
      orderItem.price = (parseFloat(item.price) * quantity).toString();
    }
    
    orderItem.purchased = false;
    orderItem.reference = this.referenceService.currentreference();

    let weight = item.rate * userInput;
    orderItem.weight = (weight.toFixed(2)).toString() + ' grams';

    console.log((weight.toFixed(2)).toString() + ' grams');

    if(item.weight == ''){
      item.weight = null;
    }

    if(item.weight != null){
      orderItem.weight = parseFloat(item.weight).toFixed(2) + ' grams';
    }

    if(orderItem.weight == '0 grams'){
      orderItem.weight = '-';      
    }

    this.tempOrderItems.push(orderItem);
    localStorage.setItem('ordered', JSON.stringify(this.tempOrderItems));

    this.reload();
  }

  calculateTotal() {
    if(this.block == 0){
      this.orderItems = this.getOrders();

      if (this.orderItems != null) {
        for (var i = 0; i < this.orderItems.length; i++) {
          this.total += parseFloat(this.orderItems[i].price); 
        }
      }

      this.total = Math.round((this.total + Number.EPSILON) * 100) / 100;
      console.log(this.orderItems);
      let num = this.total;
      let n = num.toFixed(2);

      this.totalDisplay = n;
      this.totalUsd = this.total * 0.0906750;
      this.totalUsd = Math.round((this.totalUsd + Number.EPSILON) * 100) / 100;
      //Stripe Button
      this.block = 1;
    }             
  }
  
  back(){
    this.router.navigateByUrl('/menu/' + this.referenceService.currentBranch() + '_' + this.referenceService.currentreference());
  }

  removeItem(item: OrderItem){
    this.orderItems = this.getOrders();

    let filteredItems = this.orderItems.filter(element => element.id_O != item.id_O);

    console.log(filteredItems);

    this.orderItems = filteredItems;

    localStorage.setItem('ordered', JSON.stringify(this.orderItems));
    this.reload();
  }

  reload(){
    //window.location.reload();//look for better reload method
    this.block = 0;
    this.total = 0;
    this.calculateTotal();
    this.ngOnInit();
  }

  successfulPurchase() {
    let orders = this.getOrders();
    for (let i = 0; i < orders.length; i++) {
      orders[i].phoneNumber = this.model.phoneNumber.toString();  
    }

    this.orderService.paidOnlineForOrder(this.getOrders());
    this.router.navigateByUrl('thankyou');
  }

  getOrders() : OrderItem[] {
    return JSON.parse(localStorage.getItem('ordered'));
  }

  confirmOrder() {
    localStorage.setItem('userPhoneNumber', this.model.phoneNumber.toString());
    this.router.navigateByUrl('checkout');          
  }

  payAtTill(){
    let orders = this.getOrders();

    for (let i = 0; i < orders.length; i++) {
      orders[i].phoneNumber = this.model.phoneNumber.toString();  
    }

    console.log(orders);
    
    this.orderService.payAtTill(orders);
  }

  isTablet(): Boolean{
    if(this.referenceService.currentreference() == 'tablet'){
      return true;
    }

    return false;
  }

  isExternalCustomer(): Boolean{
    if(this.referenceService.currentreference() == 'clientE'){
      return true;
    }

    return false;
  }

  formatAmount(amount: string){
    return parseFloat(amount.split(',').join('')).toLocaleString('en-US', {minimumFractionDigits: 2});;
  }

  call(index: number){
    window.open('tel:' + this.branchPhoneNumbers[index]);
  }
  leave() {
    this.thanks = false;
    this.router.navigateByUrl('order');
  }

}
