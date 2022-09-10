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
import { ThankyouComponent } from './thankyou/thankyou.component';
import { BillingComponent } from './billing/billing.component';
import { AccountmanagementComponent } from './accountmanagement/accountmanagement.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ResetpasswordComponent } from './resetpassword/resetpassword.component';
import { ResetComponent } from './reset/reset.component';
import { QuantityCounterComponent } from './quantity-counter/quantity-counter.component';
import { ScrollToTopComponent } from './scroll-to-top/scroll-to-top.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { ClosingTimePickerComponent } from './closing-time-picker/closing-time-picker.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { SideBarComponent } from './side-bar/side-bar.component';
import { AdminPortalComponent } from './admin-portal/admin-portal.component';
import { PieChartAdvancedComponent } from './pie-chart-advanced/pie-chart-advanced.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';

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
    ThankyouComponent,
    BillingComponent,
    AccountmanagementComponent,
    ResetpasswordComponent,
    ResetComponent,
    QuantityCounterComponent,
    ScrollToTopComponent,
    ClosingTimePickerComponent,
    SideBarComponent,
    AdminPortalComponent,
    PieChartAdvancedComponent,
    TextInputComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    SharedModule,
    NgxSpinnerModule,
    NgMultiSelectDropDownModule.forRoot()
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
