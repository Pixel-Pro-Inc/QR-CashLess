import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { Branch } from '../_models/branch';

@Injectable({
  providedIn: 'root'
})
export class BranchService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  submission(model: Branch, dir: string){
    return this.http.post(this.baseUrl + dir, model).subscribe(
      response =>{
        return response;
      });
  }
}
