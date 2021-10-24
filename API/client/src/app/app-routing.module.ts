import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin/admin.component';
import { HomeComponent } from './home/home.component';
import { KitchendashboardComponent } from './kitchendashboard/kitchendashboard.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MenuComponent } from './menu/menu.component';
import { MessagesComponent } from './messages/messages.component';
import { OrderComponent } from './order/order.component';
import { ReceiptComponent } from './receipt/receipt.component';
import { RegisterComponent } from './register/register.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'menu/:id', component: MenuComponent },
  { path: 'order', component: OrderComponent },
  { path: 'login', component: AdminComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'receipt', component: ReceiptComponent },
  { path: 'kitchen', component: KitchendashboardComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
