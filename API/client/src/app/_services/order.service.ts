import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { OrderItem } from '../_models/orderItem';
import { ReferenceService } from './reference.service';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient, private referenceService: ReferenceService) { }

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

  generateOrderNumber(): string{
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = String(today.getFullYear());

    let _today = dd + '/'+ mm + '/' + yyyy;

    let n = 1000;

    this.http.get(this.baseUrl + 'order/getnumber/' + this.referenceService.currentBranch()).pipe(
      map((response: number) => {
        n = response;        
      })
    );  

    let orderNum = _today + '_' + String(n);
    return orderNum;      
  }

  paidOnlineForOrder(orderItems: OrderItem[]){
    for (var i = 0; i < orderItems.length; i++) {
      if (!orderItems[i].purchased) {
        orderItems[i].purchased = true;
        //preparable

        return this.http.post(this.baseUrl + 'order/editorder', orderItems[i]).subscribe(response => {
          return response;
        },
          error => {
            console.log(error);
          });
      }
    }    
  }
  payAtTill(orderItems: OrderItem[]){
    for (var i = 0; i < orderItems.length; i++) {
      if (!orderItems[i].purchased) {
        //orderItems[i].; waiting for payment/ preparable

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
