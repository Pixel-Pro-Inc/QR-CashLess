import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { ToastrModule } from 'ngx-toastr';
import { SharedModule } from './_modules/shared.module';
import { MenuComponent } from './menu/menu.component';
import { OrderComponent } from './order/order.component';
import { AdminComponent } from './admin/admin.component';
import { PhotouploaderComponent } from './photouploader/photouploader.component';
import { KitchendashboardComponent } from './kitchendashboard/kitchendashboard.component';
import { ReceiptComponent } from './receipt/receipt.component';
import { MenuitemComponent } from './menuitem/menuitem.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { RegisterBranchComponent } from './register-branch/register-branch.component';
import { RestaurantBranchComponent } from './restaurant-branch/restaurant-branch.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RevenuechartComponent } from './revenuechart/revenuechart.component';
import { NgxSpinnerModule } from "ngx-spinner";
import { LoadingInterceptor } from './_interceptors/loading.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MemberDetailComponent,
    ListsComponent,
    MessagesComponent,
    MenuComponent,
    OrderComponent,
    AdminComponent,
    PhotouploaderComponent,
    KitchendashboardComponent,
    ReceiptComponent,
    MenuitemComponent,
    NotfoundComponent,
    CheckoutComponent,
    RegisterBranchComponent,
    RestaurantBranchComponent,
    LoginComponent,
    DashboardComponent,
    RevenuechartComponent,
    LoginComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    SharedModule,
    NgxSpinnerModule,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [
    {provide: HTTP_INTERCEPTORS,useClass:LoadingInterceptor, multi:true}  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
