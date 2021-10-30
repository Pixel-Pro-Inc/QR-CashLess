import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { MenuComponent } from '../menu/menu.component';
import { ReceiptComponent } from '../receipt/receipt.component';
import { RestaurantBranchComponent } from '../restaurant-branch/restaurant-branch.component';
import { MenuItem } from '../_models/menuItem';
import { RestBranch } from '../_models/RestBranch';
import { BranchesService } from './branches.service';

@Injectable({
  providedIn: 'root'
})
export class AdminRightService {
  baseUrl = 'https://localhost:5001/api/';

  //I set these as private cause of security reasons
  private menuClass: MenuComponent;
  private branchClass: RestaurantBranchComponent;

  constructor(private http: HttpClient, private branchService:BranchesService) { }

  addBranch(model:any, dir:string) {
    return this.http.post(this.baseUrl + dir, model).pipe(
      map((branch: RestBranch) => {
        if (branch) {
          localStorage.setItem(branch.name, JSON.stringify(branch));
        }
        return branch;
      })
    );
  }
  deleteBranch(model: any, dir: string) {
    return this.http.post(this.baseUrl + dir, model).pipe(
      map((branch: RestBranch) => {
        if (branch) {
          localStorage.removeItem(branch.name);
        }
        return branch;
      })
    )
  }
  checkBranch(name:string): boolean {
    this.branchClass.getStatuses();
    return this.branchClass.branchDictionary.get(name);
  }
  getbranches = () => this.branchClass.getBranches();

  /** I use a component here cause there are just too many variables i'll have to define to get the right results */
  getMenuItems = () => this.menuClass.getMenuItems();
  createMenuItem = () => this.menuClass.createMenuItem();
  /**These will be edited only by the admin */
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
