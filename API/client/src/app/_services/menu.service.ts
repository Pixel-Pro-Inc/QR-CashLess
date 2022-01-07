import { HttpClient } from '@angular/common/http';
import { error } from '@angular/compiler/src/util';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { MenuItem } from '../_models/menuItem';
import { BranchService } from './branch.service';
import { BaseServiceService } from './-base-service.service';

@Injectable({
  providedIn: 'root'
})
export class MenuService extends BaseServiceService {

  constructor(http: HttpClient) {
    super(http);
  }

  getMenuItems(dir: string, branchId: string) {
    return this.http.get(this.baseUrl + dir +'/' + branchId).pipe(
      map((response: MenuItem[]) => {
        return response;
      })
    )
  }

  createMenuItem(dir: string, menuItemDto: MenuItem, branchId: string) {
    return this.http.post(this.baseUrl + dir +'/' + branchId, menuItemDto).pipe(
      map((response: MenuItem) => {
        if (response) {
          localStorage.setItem(response.name, JSON.stringify(response)); //I changed it anyways, you migt not need to do it
        }
        return response;
      })
    )
  }
  editMenuItem(dir: string, menuItemDto: MenuItem, branchId: string) {
    return this.http.post(this.baseUrl + dir + '/' + branchId, menuItemDto).pipe(
      map((item: MenuItem) => {
        if (item) {
          localStorage.setItem(item.name, JSON.stringify(item));
        }
        return item;
      })
    );
  }
  deleteItem(model: any, branchId: string){
    return this.http.post(this.baseUrl + 'menu/delete/' + branchId, model).pipe(
      map((item: MenuItem) => {
        return item;
      })
    );
  }

}
