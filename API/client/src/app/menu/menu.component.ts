import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { MenuService } from '../_services/menu.service';
import { MenuItem } from '../_models/menuItem';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { error } from 'console';
import { HttpClient } from '@angular/common/http';
import { OrderItem } from '../_models/orderItem';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { ReferenceService } from '../_services/reference.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  routeSub: Subscription;
  authorized: boolean;
  showform = false;

  showMeals = true;
  showDrinks = false;
  showDesserts = false;

  firstCol = true;
  secondCol = false;
  thirdCol = false;

  count = 0;

  showEditing = false;
  cantOrder = false;

  imageSrc: string;
  user: User;

  model: MenuItem = {
    name: '',
    description: '',
    category: '',
    price: '',
    imgUrl: '',
    prepTime: ''
  };
  img: any = {};

  menuItems: MenuItem[] = [];
  menuItemsCol: MenuItem[] = [];
  menuItemsCol2: MenuItem[] = [];
  menuItemsCol3: MenuItem[] = [];
  baseUrl: string;

  orderItem: OrderItem = {
    id: 0,
    name: '',
    description: '',
    price: '',
    reference: '',
    fufilled: false,
    purchased: false,
    quantity: 0,
    calledforbill: false
  };

  constructor(private referenceService: ReferenceService, private route: ActivatedRoute, private menuService: MenuService, private http: HttpClient, private accountService: AccountService) { }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(
      params => {
        this.referenceService.setReference(params['id']);
      }
    );

    if (this.referenceService.currentreference == 'edit') {
      this.cantOrder = true;
      this.accountService.currentUser$.subscribe(r => {
        this.user = r;
        if (this.user.admin) {
          this.showEditing = true;
        }
      });
    }

    this.baseUrl = this.menuService.baseUrl;

    this.getMenuItems();
    this.groupItems();
  }

  order(item: MenuItem, quantity: number) {
    this.orderItem.quantity = quantity;
    this.orderItem.description = item.description;
    this.orderItem.fufilled = false;
    this.orderItem.name = item.name;
    this.orderItem.price = item.price;
    this.orderItem.purchased = false;
    this.orderItem.reference = this.referenceService.currentreference;

    console.log(this.orderItem);
    alert("Your food is on its way");

    return this.http.post(this.baseUrl + 'order/createorder', this.orderItem).subscribe(response => {
      return response;
    }, error => {
      console.log(error);
    });
  }
  
  getMenuItems() {
    this.menuService.getMenuItems('menu/getmenu').subscribe(response => {
      this.menuItems = response;
      console.log(response);
    }, error => console.log(error));    
  }

  groupItems() {
    this.menuService.getMenuItems('menu/getmenu').subscribe(response => {
      this.menuItems = response;
      for (var i = 0; i < this.menuItems.length; i++) {
        if (this.menuItems[i].category == 'Meal') {
          if (this.count == 0) {
            this.menuItemsCol.push(this.menuItems[i]);
            this.count = this.count + 1;
          }
          else if (this.count == 1) {
            this.menuItemsCol2.push(this.menuItems[i]);
            this.count = this.count + 1;
          }
          else if (this.count == 2) {
            this.menuItemsCol3.push(this.menuItems[i]);
            this.count = 0;
          }
        }        
      }
      this.count = 0;
      for (var i = 0; i < this.menuItems.length; i++) {
        if (this.menuItems[i].category == 'Drink') {
          if (this.count == 0) {
            this.menuItemsCol.push(this.menuItems[i]);
            this.count = this.count + 1;
          }
          else if (this.count == 1) {
            this.menuItemsCol2.push(this.menuItems[i]);
            this.count = this.count + 1;
          }
          else if (this.count == 2) {
            this.menuItemsCol3.push(this.menuItems[i]);
            this.count = 0;
          }
        }
      }
      this.count = 0;
      for (var i = 0; i < this.menuItems.length; i++) {
        if (this.menuItems[i].category == 'Dessert') {
          if (this.count == 0) {
            this.menuItemsCol.push(this.menuItems[i]);
            this.count = this.count + 1;
          }
          else if (this.count == 1) {
            this.menuItemsCol2.push(this.menuItems[i]);
            this.count = this.count + 1;
          }
          else if (this.count == 2) {
            this.menuItemsCol3.push(this.menuItems[i]);
            this.count = 0;
          }
        }
      }

      console.log(this.menuItemsCol, this.menuItemsCol2, this.menuItemsCol3);
    }, error => console.log(error));

    
  }

  createMenuItem() {
    this.formToggle();
    this.model.imgUrl = this.imageSrc;
    console.log(this.model);
    this.menuService.createMenuItem('menu/createitem', this.model).subscribe(response => {
      console.log(response);
      window.location.reload();
    })    
  }

  categoryMeals() {
    this.showMeals = true;
    this.showDrinks = false;
    this.showDesserts = false;
  }
  categoryDesserts() {
    this.showMeals = false;
    this.showDrinks = false;
    this.showDesserts = true;
  }
  categoryDrinks() {
    this.showMeals = false;
    this.showDrinks = true;
    this.showDesserts = false;
  }

  cancel() {
    this.formToggle();
  }

  formToggle() {
    this.showform = !this.showform;
  }

  onFileChange(event) {
    const reader = new FileReader();

    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      reader.readAsDataURL(file);

      reader.onload = () => {

        this.imageSrc = reader.result as string;
      };

    }
  }

}
