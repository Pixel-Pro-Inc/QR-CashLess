import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { OrderItem } from '../_models/orderItem';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-kitchendashboard',
  templateUrl: './kitchendashboard.component.html',
  styleUrls: ['./kitchendashboard.component.css']
})
export class KitchendashboardComponent implements OnInit {
  baseUrl: string;
  orderItems: OrderItem[] = [];
  groupedItems: OrderItem[][] = [[]];
  addedRefs = [''];

  constructor(private accountService: AccountService, private http: HttpClient) { }

  ngOnInit(): void {
    this.baseUrl = this.accountService.baseUrl;
    this.getOrders().subscribe(response => { this.orderItems = response; console.log(response); });
    //this.groupOrders();
  }
  groupOrders() {
    /*this.getOrders().subscribe(response => {
      this.orderItems = response;
      for (var i = 0; i < this.orderItems.length; i++) {
        if (this.addedRefs.includes(this.orderItems[i].reference) == false) {
          for (var x = 0; x < this.orderItems.length; x++) {
            if (this.orderItems[i].reference == this.orderItems[x].reference) {
              if (this.orderItems[i].fufilled == this.orderItems[x].fufilled) {
                this.groupedItems[i].push(this.orderItems[x]);
                this.addedRefs.push(this.orderItems[i].reference);
              }
            }
          }
        }
        else {
          for (var x = 0; x < this.orderItems.length; x++) {
            if (this.orderItems[i].reference == this.orderItems[x].reference) {
              if (this.orderItems[i].fufilled == this.orderItems[x].fufilled) {
                this.groupedItems[i].push(this.orderItems[x]);
              }
            }
          }
        }
      }

      console.log(this.groupedItems);
    });*/    
  }
  getOrders() {
    return this.http.get(this.baseUrl + 'order/getorders').pipe(
      map((response: OrderItem[]) => {
        return response;
      })
    )
  }
  editOrder(item: OrderItem) {
    item.fufilled = true;

    return this.http.post(this.baseUrl + 'order/editorder', item).subscribe(response => {
      return response;
    },
      error => {
        console.log(error);
      });
  }

  finishPayment(item: OrderItem) {
    item.purchased = true;

    return this.http.post(this.baseUrl + 'order/editorder', item).subscribe(response => {
      return response;
    },
      error => {
        console.log(error);
      });
  }

}
