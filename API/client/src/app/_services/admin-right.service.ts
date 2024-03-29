import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { MenuComponent } from '../menu/menu.component';
import { ReceiptComponent } from '../receipt/receipt.component';
import { RestaurantBranchComponent } from '../restaurant-branch/restaurant-branch.component';
import { MenuItem } from '../_models/menuItem';
import { BaseServiceService } from './-base-service.service';

@Injectable({
  providedIn: 'root'
})
export class AdminRightService extends BaseServiceService {

  //I set these as private cause of security reasons
  private menuClass: MenuComponent;
  private branchClass: RestaurantBranchComponent;
  private recieptDictionary: Map<number, ReceiptComponent>;

  constructor(http: HttpClient) {
    super(http);
  }
  
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

  pushtoReceiptBucket(model: any, dir: string) {
    return this.http.post(this.baseUrl + dir, model).pipe(
      map((sliplist: Map<number, ReceiptComponent> ) => {
        if (sliplist) {
          localStorage.setItem(Date.now.toString(), JSON.stringify(sliplist)); //I'm using date here cause its a batch of receipts with different times, so its better to identify them when they are sent up
        }
        return sliplist;
      })
    );
    
  }
  getreceiptList(model: any, dir: string) {
    return this.http.get(this.baseUrl + dir).pipe(
      map((response: Map<number, ReceiptComponent>) => {
        return response;
      })
    )
  }

  getreceipt = (slipNumber: number) => this.recieptDictionary.get(slipNumber);
  //pushtoReceiptDic = (slip: ReceiptComponent) => this.recieptDictionary.set(slip.invoiceNum, slip);


}
