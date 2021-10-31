import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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

  showMeats = true;
  showDrinks = false;
  showDesserts = false;

  showEditing = false;
  cantOrder = false;

  imageSrc: string;
  user: User;

  model: MenuItem = {
    name: '',
    description: '',
    category: '',
    price: '0',
    imgUrl: '',
    prepTime: '',
    minimumPrice: 0,
    rate: 0,
    availability: false
  };
  img: any = {};

  menuItems: MenuItem[] = [];
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
    calledforbill: false,
    weight: ''
  };

  orderItems: OrderItem[] = [];
  showSides: boolean;

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
  }
  
  getMenuItems() {
    this.menuService.getMenuItems('menu/getmenu').subscribe(response => {
      this.menuItems = response;
      console.log(response);
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
  

  categoryMeats() {
    this.showMeats = true;
    this.showSides = false;
    this.showDrinks = false;
    this.showDesserts = false;
  }
  categorySides() {
    this.showSides = true;
    this.showMeats = false;
    this.showDrinks = false;
    this.showDesserts = false;
  }
  categoryDesserts() {
    this.showMeats = false;
    this.showSides = false;
    this.showDrinks = false;
    this.showDesserts = true;
  }
  categoryDrinks() {
    this.showMeats = false;
    this.showSides = false;
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
