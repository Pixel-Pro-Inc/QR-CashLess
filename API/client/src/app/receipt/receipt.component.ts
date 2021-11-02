import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Local } from 'protractor/built/driverProviders';
import { map } from 'rxjs/operators';
import { AdminComponent } from '../admin/admin.component';
import { OrderItem } from '../_models/orderItem';
import { AccountService } from '../_services/account.service';
import { ReferenceService } from '../_services/reference.service';

@Component({
  selector: 'app-receipt',
  templateUrl: './receipt.component.html',
  styleUrls: ['./receipt.component.css']
})
export class ReceiptComponent implements OnInit {
  orders: OrderItem[]
  orderItems: OrderItem[] = [];
  baseUrl: string;
  invoiceNum: number;
  date: any;
  total = 0;
  private adminComponent: AdminComponent;

  constructor(private http: HttpClient, private accountService: AccountService, private referenceService: ReferenceService) { }

  ngOnInit(): void {
    this.baseUrl = this.accountService.baseUrl;
    this.invoiceNum = Math.random() * 1000000;
    this.invoiceNum = Math.round(this.invoiceNum);
    this.date = Date.now();

    this.getOrders().subscribe(response => {
      this.orders = response;

      for (var i = 0; i < this.orders.length; i++) {
        if (this.orders[i].reference == this.referenceService.currentreference()) {
          if (this.orders[i].purchased) {
            this.orderItems.push(this.orders[i]);
          }          
        }
      };

      for (var i = 0; i < this.orderItems.length; i++) {
        if (this.orderItems[i].reference == this.referenceService.currentreference()) {
          if (this.orderItems[i].purchased) {
            this.total += parseFloat(this.orderItems[i].price);
          }
        }
      };

      console.log(this.orders, this.orderItems);
    });

    this.adminComponent.pushreceipt(this);
  }
  getOrders() {
    return this.http.get(this.baseUrl + 'order/getorders').pipe(
      map((response: OrderItem[]) => {
        return response;
      })
    )
  }
}
