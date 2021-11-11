import { HttpClient } from '@angular/common/http';
import { Injectable} from '@angular/core';
import { Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { OrderItem } from '../_models/orderItem';
import { ReferenceService } from './reference.service';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  //baseUrl = 'https://localhost:5001/api/';
  baseUrl = 'https://rodizioexpress.azurewebsites.net/api/';

  constructor(private http: HttpClient, private router: Router, private referenceService: ReferenceService) { }

  createOrder(orderItems: OrderItem[]){
    
    let bId = this.referenceService.currentBranch();

    this.http.post(this.baseUrl + 'order/createorder/' + bId, orderItems).subscribe(response => {
      localStorage.setItem('ordered', JSON.stringify(response));
      console.log(response);

      //Receipt Page
      this.router.navigateByUrl('/receipt');
    },
    error => {
      console.log(error);
    });  
  }

  paidOnlineForOrder(orderItems: OrderItem[]){
    for (var i = 0; i < orderItems.length; i++) {
      if (!orderItems[i].purchased) {
        orderItems[i].purchased = true;
        orderItems[i].preparable = true;
      }
    }

    this.createOrder(orderItems);
  }
  payAtTill(orderItems: OrderItem[]){
    for (var i = 0; i < orderItems.length; i++) {
      if (!orderItems[i].purchased) {
        orderItems[i].waitingForPayment = true;
        orderItems[i].preparable = true;
      }
    }
    
    this.createOrder(orderItems);
  }
}
