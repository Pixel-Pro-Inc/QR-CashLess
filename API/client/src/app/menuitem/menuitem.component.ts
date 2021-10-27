import { Component, Input, OnInit } from '@angular/core';
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

  userInput: number = 0;

  constructor() { }

  ngOnInit(): void {
    
  }

  clicked(item: MenuItem, quantity: number){
    this.orderView.updateOrderView(item, quantity, this.userInput);
  }

}
