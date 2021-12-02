import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { OrderComponent } from '../order/order.component';
import { MenuItem } from '../_models/menuItem';

@Component({
  selector: 'app-menuitem',
  templateUrl: './menuitem.component.html',
  styleUrls: ['./menuitem.component.css']
})
export class MenuitemComponent implements OnInit {
  @Input() menuItems : MenuItem[];
  @Input() category : string;
  @Input() cantOrder: boolean;
  @Input() orderView: OrderComponent;

  userInput: number[] = [];

  constructor(private toastr: ToastrService) { }

  ngOnInit(): void {
    
  }

  clicked(item: MenuItem, quantity: number, usersInput: number){
    console.log(item);

    this.toastr.success(item.name + ' was added to your order.');
    this.orderView.updateOrderView(item, quantity, usersInput);    
  }

}
