import { __decorate } from "tslib";
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AdminPortalComponent } from './admin-portal/admin-portal.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { ClosingTimePickerComponent } from './closing-time-picker/closing-time-picker.component';
import { LoginComponent } from './login/login.component';
import { MenuComponent } from './menu/menu.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { ReceiptComponent } from './receipt/receipt.component';
import { RegisterBranchComponent } from './register-branch/register-branch.component';
import { RegisterComponent } from './register/register.component';
import { ResetComponent } from './reset/reset.component';
import { ResetpasswordComponent } from './resetpassword/resetpassword.component';
import { RestaurantBranchComponent } from './restaurant-branch/restaurant-branch.component';
import { ThankyouComponent } from './thankyou/thankyou.component';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';
import { DevGuard } from './_guards/dev.guard';
const routes = [
    { path: '', component: RestaurantBranchComponent },
    { path: 'menu/:id', component: MenuComponent },
    { path: 'setclosetime', component: ClosingTimePickerComponent, canActivate: [AdminGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'register/branch', component: RegisterBranchComponent, canActivate: [DevGuard] },
    { path: 'register/admin', component: RegisterComponent, canActivate: [DevGuard] },
    { path: 'register/user', component: RegisterComponent, canActivate: [AdminGuard] },
    { path: 'admin-portal', component: AdminPortalComponent, canActivate: [AdminGuard] },
    { path: 'receipt', component: ReceiptComponent },
    { path: 'branches', component: RestaurantBranchComponent },
    { path: 'checkout', component: CheckoutComponent },
    { path: 'password/reset', component: ResetpasswordComponent },
    { path: 'password/reset/success', component: ResetComponent, canActivate: [AuthGuard] },
    { path: 'thankyou', component: ThankyouComponent },
    { path: '**', component: NotfoundComponent, pathMatch: 'full' }
];
let AppRoutingModule = class AppRoutingModule {
};
AppRoutingModule = __decorate([
    NgModule({
        imports: [RouterModule.forRoot(routes)],
        exports: [RouterModule]
    })
], AppRoutingModule);
export { AppRoutingModule };
//# sourceMappingURL=app-routing.module.js.map