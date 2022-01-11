import { HttpClient } from '@angular/common/http';
import { error } from '@angular/compiler/src/util';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { MenuItem } from '../_models/menuItem';
import { BranchService } from './branch.service';
import { BaseServiceService } from './-base-service.service';
import { BusyService } from './busy.service';

@Injectable({
  providedIn: 'root'
})
export class MenuService extends BaseServiceService {

  constructor(http: HttpClient, private busyService: BusyService) {
    super(http);
  }

  getMenuItems(dir: string, branchId: string) {
    this.busyService.busy();
    return this.http.get(this.baseUrl + dir +'/' + branchId).pipe(
      map((response: MenuItem[]) => {
        this.busyService.idle();
        return response;
      })
    )
  }

  createMenuItem(dir: string, menuItemDto: MenuItem, branchId: string) {
    this.busyService.busy();
    return this.http.post(this.baseUrl + dir +'/' + branchId, menuItemDto).pipe(
      map((response: MenuItem) => {
        if (response) {
          localStorage.setItem(response.name, JSON.stringify(response)); //I changed it anyways, you migt not need to do it
        }
        this.busyService.idle();
        return response;
      })
    )
  }
  editMenuItem(dir: string, menuItemDto: MenuItem, branchId: string) {
    this.busyService.busy();
    return this.http.post(this.baseUrl + dir + '/' + branchId, menuItemDto).pipe(
      map((item: MenuItem) => {
        if (item) {
          localStorage.setItem(item.name, JSON.stringify(item));
        }
        this.busyService.idle();
        return item;
      })
    );
  }
  deleteItem(model: any, branchId: string){
    this.busyService.busy();
    return this.http.post(this.baseUrl + 'menu/delete/' + branchId, model).pipe(
      map((item: MenuItem) => {
        this.busyService.idle();
        return item;
      })
    );
  }

}
