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
import { ToastrService } from 'ngx-toastr';

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
    availability: false,
    id: 0,
    publicId: ''
  };
  img: any = {};

  menuItems: MenuItem[] = [];

  orderItem: OrderItem = {
    name: '',
    description: '',
    reference: '',
    price: '',
    weight: '',
    fufilled: false,
    purchased: false,
    paymentMethod: '',
    preparable: false,
    waitingForPayment: false,
    quantity: 0,
    orderNumber: '',
    phoneNumber: '',
    category: '',
    prepTime: 0
  };

  orderItems: OrderItem[] = [];
  showSides: boolean;

  showEditForm = false;

  model1: any = {};

  menu: MenuComponent = this;


  constructor(private referenceService: ReferenceService, private route: ActivatedRoute, private menuService: MenuService, private http: HttpClient, private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.user = JSON.parse(localStorage.getItem('user'));

    if (this.user != null) {
      this.referenceService.setRefExplicit('edit');

      this.cantOrder = true;

      this.accountService.currentUser$.subscribe(r => {
        this.user = r;
        if (this.user.admin) {
          this.showEditing = true;
        }
      });
    }

    if(this.user == null){
      this.routeSub = this.route.params.subscribe(
        params => {
          this.referenceService.setReference(params['id']);
        }
      );
    }

    this.getMenuItems(this.referenceService.currentBranch());

    console.log(this.showEditing);
  }
  
  getMenuItems(branchId: string) {
    //console.log(branchId);
    this.menuService.getMenuItems('menu/getmenu', branchId).subscribe(response => {
      this.menuItems = response;
      console.log(response);
    }, error => console.log(error));    
  }
  createMenuItem() {
    let branchId = this.referenceService.currentBranch();

    //console.log('branch id ' + branchId);
    
    this.formToggle();
    this.model.imgUrl = this.imageSrc;
    console.log(this.model);

    this.toastr.success("Menu item added successfully");
    
    this.menuService.createMenuItem('menu/createitem', this.model, branchId).subscribe(response => {      
      window.location.reload();
    }, 
    error =>{
      console.log(error);
    })    
  }

  editMenuItem() {
    let branchId = this.referenceService.currentBranch();

    console.log('branch id ' + branchId);
    
    this.formToggle();
    this.model1.imgUrl = this.img.image == null? this.model1.imgUrl : this.imageSrc;

    console.log(this.model1);

    this.toastr.success("Menu item edited successfully");
    
    this.menuService.editMenuItem('menu/edititem', this.model1, branchId).subscribe(response => {      
      window.location.reload();
    }, 
    error =>{
      console.log(error);
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

  cancel2() {
    this.showEditForm = false;
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

  toBottom(){
    console.log('something');
    window.scrollTo(0, document.body.scrollHeight);
  }

}
