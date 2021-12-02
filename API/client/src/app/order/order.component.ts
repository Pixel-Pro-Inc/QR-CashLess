import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { map } from 'rxjs/operators';
import { OrderItem } from '../_models/orderItem';
import { AccountService } from '../_services/account.service';
import { render } from 'creditcardpayments/creditCardPayments';
import { ReferenceService } from '../_services/reference.service';
import { error } from '@angular/compiler/src/util';
import { NavigationEnd, Router } from '@angular/router';
import { OrderService } from '../_services/order.service';
import { MenuItem } from '../_models/menuItem';
import { Branch } from '../_models/branch';
import { BranchService } from '../_services/branch.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  @Input() showPaymentOptions : boolean;

  total = 0.00;
  totalDisplay = '';
  totalUsd = 0.00;

  
  orderItems : OrderItem[];
  tempOrderItems : OrderItem[] = [];

  model: any = {};

  block = 0;

  onlineOrderAvailable: Boolean;
  branchPhoneNumber: number;

  constructor(private router: Router, private orderService: OrderService, private referenceService: ReferenceService, private branchService: BranchService) { }

  ngOnInit(): void {
    this.model.phoneNumber = JSON.parse(localStorage.getItem('userPhoneNumber'));

    this.orderItems = this.getOrders();    
    
    this.branchService.getRestBranches('branch/getbranches').subscribe(response => {
      let RestBranches: Branch[] = response;

      for (let i = 0; i < RestBranches.length; i++) {
        if(RestBranches[i].id == this.referenceService.currentBranch()){
          if(RestBranches[i].lastActive < 30){
            this.onlineOrderAvailable = true;

            this.branchPhoneNumber = RestBranches[i].phoneNumber;
          }
        }
      }
      
    });
  }

  showOptions(){
    localStorage.setItem('userPhoneNumber', this.model.phoneNumber.toString());

    window.location.reload();
  }

  public updateOrderView(item: MenuItem, quantity: number, userInput: number) {
    if(this.getOrders() != null){
      this.tempOrderItems = this.getOrders();//update list to latest values
    }    

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
        paymentMethod: ''
    };

    orderItem.quantity = quantity;
    orderItem.description = item.description;
    orderItem.fufilled = false;
    orderItem.name = item.name;

    if(item.category == 'Meat'){
      orderItem.price = userInput.toString();
    }

    if(item.category != 'Meat'){
      orderItem.price = item.price;
    }
    
    orderItem.purchased = false;
    orderItem.reference = this.referenceService.currentreference();

    let weight = item.rate * userInput;
    orderItem.weight = weight.toFixed(2) + ' grams';

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
      if(this.showPaymentOptions){
        render(
          {
            id: "#myPaypalButtons",
            currency: "USD",
            value: this.totalUsd.toString(),
            onApprove: (details) => {              
              this.successfulPurchase();
            },
          }
        );   
      }
      this.block = 1;
    }             
  }

  removeItem(item: OrderItem){
    this.orderItems = this.getOrders();
    this.orderItems.splice(this.orderItems.indexOf(item));

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

  call(){
    window.open('tel:' + this.branchPhoneNumber);
  }  
}
