import { __decorate } from "tslib";
import { Component } from '@angular/core';
let MenuComponent = class MenuComponent {
    constructor(referenceService, route, menuService, http, accountService, toastr) {
        this.referenceService = referenceService;
        this.route = route;
        this.menuService = menuService;
        this.http = http;
        this.accountService = accountService;
        this.toastr = toastr;
        this.dropdownList = [];
        this.selectedItems = [];
        this.dropdownList_1 = [];
        this.selectedItems_1 = [];
        this.dropdownList_2 = [];
        this.selectedItems_2 = [];
        this.dropdownSettings = {
            singleSelection: false,
            idField: 'item_id',
            textField: 'item_text',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 3,
            allowSearchFilter: true
        };
        this.showform = false;
        this.standardCheck = {};
        this.showMeats = true;
        this.showDrinks = false;
        this.showDesserts = false;
        this.showEditing = false;
        this.cantOrder = false;
        this.subCategories = [];
        this.flavours = [];
        this.meatTemperatures = [];
        this.sauces = [];
        this.subCategoryCollapse = [];
        this.model = {
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
            publicId: '',
            subCategory: '',
            weight: '',
            flavours: [],
            meatTemperatures: [],
            sauces: [],
            selectedFlavour: '',
            selectedMeatTemperature: '',
            selectedSauces: []
        };
        this.img = {};
        this.menuItems = [];
        this.orderItem = {
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
            prepTime: 0,
            id_O: '',
            flavour: '',
            meatTemperature: '',
            sauces: [],
            subCategory: ''
        };
        this.orderItems = [];
        this.showEditForm = false;
        this.model1 = {};
        this.menu = this;
    }
    ngOnInit() {
        this.standardCheck.check = true;
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
        if (this.user == null) {
            this.routeSub = this.route.params.subscribe(params => {
                this.referenceService.setReference(params['id']);
            });
        }
        this.getMenuItems(this.referenceService.currentBranch());
        this.menuService.getSubCategories().subscribe(response => {
            let m = {};
            m = response;
            m.forEach(element => {
                if (!this.subCategories.includes(element)) {
                    this.subCategories.push(element);
                }
            });
            this.subCategories.forEach(element => {
                this.subCategoryCollapse.push(false);
            });
        });
        //Flavours
        this.menuService.getFlavours().subscribe(response => {
            let f = {};
            f = response;
            f.forEach(element => {
                if (!this.flavours.includes(element)) {
                    this.flavours.push(element);
                    this.dropdownList.push(element);
                }
            });
        });
        //Meat Temperature
        this.menuService.getMeatTemperatures().subscribe(response => {
            let f = {};
            f = response;
            f.forEach(element => {
                if (!this.meatTemperatures.includes(element)) {
                    this.meatTemperatures.push(element);
                    this.dropdownList_1.push(element);
                }
            });
        });
        //Sauce
        this.menuService.getSauces().subscribe(response => {
            let f = {};
            f = response;
            f.forEach(element => {
                if (!this.sauces.includes(element)) {
                    this.sauces.push(element);
                    this.dropdownList_2.push(element);
                }
            });
        });
        console.log(this.showEditing);
        //Means you havent visited this site until the lastest update on this browser
        if (localStorage.getItem('hasEverVisited') == null) {
            let empty = [];
            localStorage.setItem('ordered', JSON.stringify(empty));
            localStorage.setItem('hasEverVisited', 'true');
        }
    }
    createNewFlavour(flavour) {
        this.menuService.createFlavour(flavour);
    }
    createNewMeatTemperature(meattemperature) {
        this.menuService.createMeatTemperature(meattemperature);
    }
    createNewSauce(sauce) {
        this.menuService.createSauce(sauce);
    }
    getMenuItems(branchId) {
        //console.log(branchId);
        this.menuService.getMenuItems('menu/getmenu', branchId).subscribe(response => {
            this.menuItems = response;
            this.menuItems.forEach(element => {
                element.selectedFlavour = 'None';
                element.selectedMeatTemperature = 'Well Done';
                element.selectedSauces = ['Lemon & Garlic'];
            });
            console.log(response);
        }, error => console.log(error));
    }
    createMenuItem() {
        let branchId = this.referenceService.currentBranch();
        //console.log('branch id ' + branchId);
        this.formToggle();
        this.model.imgUrl = this.imageSrc;
        this.model.flavours = this.selectedItems;
        this.model.meatTemperatures = this.selectedItems_1;
        this.model.sauces = this.selectedItems_2;
        //Forcing it to always be a string before it touches the api because sometimes its a number
        this.model.weight = this.model.weight.toString();
        console.log(this.model);
        this.toastr.success("Menu item added successfully");
        this.menuService.createMenuItem('menu/createitem', this.model, branchId).subscribe(response => {
            window.location.reload();
        }, error => {
            console.log(error);
        });
    }
    editMenuItem() {
        let branchId = this.referenceService.currentBranch();
        //console.log('branch id ' + branchId);
        this.formToggle();
        this.model1.flavours = this.selectedItems;
        this.model1.meatTemperatures = this.selectedItems_1;
        this.model1.sauces = this.selectedItems_2;
        //Forcing it to always be a string before it touches the api because sometimes its a number
        this.model1.weight = this.model.weight.toString();
        this.model1.imgUrl = this.img.image == null ? this.model1.imgUrl : this.imageSrc;
        console.log(this.model1);
        this.toastr.success("Menu item edited successfully");
        this.menuService.editMenuItem('menu/edititem', this.model1, branchId).subscribe(response => {
            window.location.reload();
        }, error => {
            console.log(error);
        });
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
                this.imageSrc = reader.result;
            };
        }
    }
};
MenuComponent = __decorate([
    Component({
        selector: 'app-menu',
        templateUrl: './menu.component.html',
        styleUrls: ['./menu.component.css']
    })
], MenuComponent);
export { MenuComponent };
//# sourceMappingURL=menu.component.js.map