import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { map } from 'rxjs/operators';
import { OrderItem } from '../_models/orderItem';
import { AccountService } from '../_services/account.service';
import { render } from 'creditcardpayments/creditCardPayments';
import { ReferenceService } from '../_services/reference.service';
import { error } from '@angular/compiler/src/util';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  total = 0.00;
  totalUsd = 0.00;
  block = 0;
  block1 = 0;
  orderItems: OrderItem[];
  baseUrl: string;

  activeOrder = false;
  constructor(private accountService: AccountService, private http: HttpClient, public referenceService: ReferenceService, private router: Router) { }

  ngOnInit(): void {
    this.baseUrl = this.accountService.baseUrl;
    this.getOrders().subscribe(response => {
      this.orderItems = response;
      console.log(response);
    });
  }

  hasOrdered() {
    if (this.block1 == 0) {
      for (var i = 0; i < this.orderItems.length; i++) {
        if (this.orderItems[i].reference == this.referenceService.currentreference) {
          if (!this.orderItems[i].purchased) {
            this.activeOrder = true;
          }
        }
      }
      this.block1 = 1;
    }    
  }

  calculateTotal() {
    if (this.block == 0) {
      this.getOrders().subscribe(response => {
        this.orderItems = response;
        console.log(response);

        console.log(this.orderItems);
        if (this.orderItems != null) {
          for (var i = 0; i < this.orderItems.length; i++) {
            if (this.orderItems[i].reference == this.referenceService.currentreference) {
              if (!this.orderItems[i].purchased) {
                this.total += parseFloat(this.orderItems[i].price);
              }
            }
          }
        }

        this.total = Math.round((this.total + Number.EPSILON) * 100) / 100;
        this.totalUsd = this.total * 0.0906750;
        this.totalUsd = Math.round((this.totalUsd + Number.EPSILON) * 100) / 100;

        console.log(this.totalUsd.toString());

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
      });     

      this.block = 1;
    }    
  }

  successfulPurchase() {
    this.getOrders().subscribe(response => {
      this.orderItems = response;
      for (var i = 0; i < this.orderItems.length; i++) {
        if (this.orderItems[i].reference == this.referenceService.currentreference) {
          if (!this.orderItems[i].purchased) {
            this.orderItems[i].purchased = true;

            return this.http.post(this.baseUrl + 'order/editorder', this.orderItems[i]).subscribe(response => {
              return response;
            },
              error => {
                console.log(error);
              });
          }
        }
      }
      alert("Transaction Successfull");
    });
    
  }

  getOrders() {
    return this.http.get(this.baseUrl + 'order/getorders').pipe(
      map((response: OrderItem[]) => {
        return response;
      })
    )
  }

  callForBill() {
    this.getOrders().subscribe(response => {
      this.orderItems = response;
      for (var i = 0; i < this.orderItems.length; i++) {
        if (this.orderItems[i].reference == this.referenceService.currentreference) {
          if (!this.orderItems[i].purchased) {
            this.orderItems[i].calledforbill = true;

            return this.http.post(this.baseUrl + 'order/editorder', this.orderItems[i]).subscribe(response => {
              return response;              
            },
              error => {
                console.log(error);
              });
          }
        }
      }

      alert('The bill is on the way');
    });        
  }

}
