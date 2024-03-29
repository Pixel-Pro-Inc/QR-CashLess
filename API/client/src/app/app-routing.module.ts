import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPortalComponent } from './admin-portal/admin-portal.component';
import { AdminComponent } from './admin/admin.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { ClosingTimePickerComponent } from './closing-time-picker/closing-time-picker.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HomeComponent } from './home/home.component';
import { KitchendashboardComponent } from './kitchendashboard/kitchendashboard.component';
import { ListsComponent } from './lists/lists.component';
import { LoginComponent } from './login/login.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MenuComponent } from './menu/menu.component';
import { MessagesComponent } from './messages/messages.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { OrderComponent } from './order/order.component';
import { ReceiptComponent } from './receipt/receipt.component';
import { RegisterBranchComponent } from './register-branch/register-branch.component';
import { RegisterComponent } from './register/register.component';
import { ResetComponent } from './reset/reset.component';
import { ResetpasswordComponent } from './resetpassword/resetpassword.component';
import { RestaurantBranchComponent } from './restaurant-branch/restaurant-branch.component';
import { SideBarComponent } from './side-bar/side-bar.component';
import { ThankyouComponent } from './thankyou/thankyou.component';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';
import { DevGuard } from './_guards/dev.guard';

const routes: Routes = [
  { path: '', component: RestaurantBranchComponent },
  { path: 'menu/:id', component: MenuComponent },
  { path: 'setclosetime', component: ClosingTimePickerComponent, canActivate: [AdminGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register/branch', component: RegisterBranchComponent, canActivate: [DevGuard]},
  { path: 'register/admin', component: RegisterComponent, canActivate: [DevGuard]},
  { path: 'register/user', component: RegisterComponent, canActivate: [AdminGuard] },
  { path: 'admin-portal', component: AdminPortalComponent, canActivate: [AdminGuard] },
  { path: 'receipt', component: ReceiptComponent },
  { path: 'branches', component: RestaurantBranchComponent},
  { path: 'checkout', component: CheckoutComponent},
  { path: 'password/reset', component: ResetpasswordComponent},
  { path: 'password/reset/success', component: ResetComponent, canActivate: [AuthGuard]},
  { path: 'thankyou', component: ThankyouComponent},
  { path: '**', component: NotfoundComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
