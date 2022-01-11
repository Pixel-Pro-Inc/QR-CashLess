import { HttpClient } from '@angular/common/http';
import { Injectable} from '@angular/core';
import { Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { OrderItem } from '../_models/orderItem';
import { BaseServiceService } from './-base-service.service';
import { BusyService } from './busy.service';
import { ReferenceService } from './reference.service';

@Injectable({
  providedIn: 'root'
})
export class OrderService extends BaseServiceService {

  constructor(http: HttpClient, private router: Router, private referenceService: ReferenceService, private busyService: BusyService) {
    super(http);
  }

  createOrder(orderItems: OrderItem[]){
    this.busyService.busy();
    
    let bId = this.referenceService.currentBranch();

    this.http.post(this.baseUrl + 'order/createorder/' + bId, orderItems).subscribe(response => {
      localStorage.setItem('ordered', JSON.stringify(response));
      console.log(response);

      //Receipt Page
      if(this.referenceService.currentreference() != 'tablet'){
        this.router.navigateByUrl('/receipt');
        return;
      }

      let x: string = '';

      x = this.referenceService.currentBranch() + '_' + this.referenceService.currentreference();

      let empty: OrderItem[] = [];
      localStorage.setItem('ordered', JSON.stringify(empty));

      if(this.referenceService.currentreference() == "tablet"){
        localStorage.setItem('userPhoneNumber', JSON.stringify(null));
      }      

      this.busyService.idle();
      
      this.router.navigateByUrl('/menu/' + x);
    },
    error => {
      console.log(error);
    });  
  }

  paidOnlineForOrder(orderItems: OrderItem[]){
    for (var i = 0; i < orderItems.length; i++) {
      if (!orderItems[i].purchased) {
        orderItems[i].purchased = true;
        orderItems[i].paymentMethod = 'online';
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
