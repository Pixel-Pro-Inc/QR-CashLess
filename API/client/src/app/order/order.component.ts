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

  block = 0;

  constructor(private router: Router, private orderService: OrderService, 
    private referenceService: ReferenceService) 
    {
    }

  ngOnInit(): void {
    this.orderItems = this.getOrders();    
  }

  public updateOrderView(item: MenuItem, quantity: number, userInput: number) {
    if(this.getOrders() != null){
      this.tempOrderItems = this.getOrders();//update list to latest values
    }    

    let orderItem : OrderItem = {
      id: 0,
      name: '',
      description: '',
      price: '',
      reference: '',
      fufilled: false,
      purchased: false,
      quantity: 0,
      calledforbill: false,
      weight: ''
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
    orderItem.reference = this.referenceService.currentreference;

    let weight = item.rate * userInput;
    orderItem.weight = weight.toString() + ' grams';

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
    this.orderService.paidOnlineForOrder(this.getOrders());    
  }

  getOrders() : OrderItem[] {
    return JSON.parse(localStorage.getItem('ordered'));
  }

  confirmOrder() {
    this.orderService.createOrder(this.getOrders());              
  }

  payAtTill(){
    //Waiting for payment
  }
}