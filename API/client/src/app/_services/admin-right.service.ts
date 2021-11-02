import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { MenuComponent } from '../menu/menu.component';
import { ReceiptComponent } from '../receipt/receipt.component';
import { RestaurantBranchComponent } from '../restaurant-branch/restaurant-branch.component';
import { MenuItem } from '../_models/menuItem';

@Injectable({
  providedIn: 'root'
})
export class AdminRightService {
  baseUrl = 'https://localhost:5001/api/';

  //I set these as private cause of security reasons
  private menuClass: MenuComponent;
  private branchClass: RestaurantBranchComponent;

  constructor(private http: HttpClient) { }
  
  getbranches = () => this.branchClass.getBranches();
  
  deleteMenuItem(model: any, dir: string) {
    return this.http.post(this.baseUrl + dir, model).pipe(
      map((item: MenuItem) => {
        if (item) {
          localStorage.removeItem(item.name);
        }
        return item;
      })
    )
  }
  editMenuItem(model: any, dir: string) {
    return this.http.post(this.baseUrl + dir, model).pipe(
      map((item: MenuItem) => {
        if (item) {
          localStorage.setItem(item.name, JSON.stringify(item)); //I'm assuming that since its the same place and name it will just overwrite
        }
        return item;
      })
    );
  }

  getrecepit() {
    //will get from service
  }

}
