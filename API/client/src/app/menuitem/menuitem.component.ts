import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { MenuComponent } from '../menu/menu.component';
import { OrderComponent } from '../order/order.component';
import { MenuItem } from '../_models/menuItem';
import { BranchService } from '../_services/branch.service';
import { MenuService } from '../_services/menu.service';
import { ReferenceService } from '../_services/reference.service';

@Component({
  selector: 'app-menuitem',
  templateUrl: './menuitem.component.html',
  styleUrls: ['./menuitem.component.css']
})
export class MenuitemComponent implements OnInit {
  @Input() menuItems : MenuItem[];
  @Input() category : string;
  @Input() subCategory : string = '';
  @Input() cantOrder: boolean;
  @Input() orderView: OrderComponent;
  @Input() menuView: MenuComponent;

  model:any = {};

  userInput: number[] = [];

  constructor(private toastr: ToastrService, private menuService: MenuService, private referenceService: ReferenceService) { }

  ngOnInit(): void {
    
  }

  clicked(item: MenuItem, usersInput: number){
    console.log(item);
    console.log(usersInput);
    console.log(this.model);

    this.toastr.success(item.name + ' was added to your order.', 'Order Confirmation', {
      positionClass: 'toast-top-right' 
   });

    this.orderView.updateOrderView(item, this.model.quantity, usersInput);  
  }

  editItem(model: any){
    this.menuView.showEditForm = true;

    this.menuView.model1 = model;
  }

  deleteItem(model: any){
    if(confirm("Are you sure you want to permanently delete " + model.name)) {
      this.menuService.deleteItem(model, this.referenceService.currentBranch()).subscribe(
        response => {
          window.location.reload();
        }
      );
    }
  }

}
