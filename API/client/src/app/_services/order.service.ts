import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OrderItem } from '../_models/orderItem';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  createOrder(orderItems: OrderItem[]){
    
    for (var i = 0; i < orderItems.length; i++) {
      this.http.post(this.baseUrl + 'order/createorder', orderItems[i]).subscribe(response => {
        return response;              
      },
        error => {
          console.log(error);
        });
    }    
  }

  paidOnlineForOrder(orderItems: OrderItem[]){
    for (var i = 0; i < orderItems.length; i++) {
      if (!orderItems[i].purchased) {
        orderItems[i].purchased = true;

        return this.http.post(this.baseUrl + 'order/editorder', orderItems[i]).subscribe(response => {
          return response;
        },
          error => {
            console.log(error);
          });
      }
    }    
  }
}
