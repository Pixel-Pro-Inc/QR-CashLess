import { HttpClient } from '@angular/common/http';
import { error } from '@angular/compiler/src/util';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { MenuItem } from '../_models/menuItem';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  getMenuItems(dir: string) {
    return this.http.get(this.baseUrl + dir).pipe(
      map((response: MenuItem[]) => {
        return response;
      })
    )
  }

  createMenuItem(dir: string, model: MenuItem) {
    return this.http.post(this.baseUrl + dir, model).pipe(
      map((response: MenuItem) => {
        if (response) {
          localStorage.setItem(response.name, JSON.stringify(response)); //I changed it anyways, you migt not need to do it
        }
        return response;
      })
    )
    /*return this.http.post(this.baseUrl + dir, model).subscribe(
      response => {
        return response;
      },
      error => {
        console.log(error);
        return error;
      }
    )*/
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

}
