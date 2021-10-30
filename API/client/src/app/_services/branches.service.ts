import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { RestBranch } from '../_models/RestBranch';

@Injectable({
  providedIn: 'root'
})
export class BranchesService {
  baseUrl = 'https://localhost:5001/api/';
  constructor(private http: HttpClient) { }

  getRestBranches(dir: string) {
    return this.http.get(this.baseUrl + dir).pipe(
      map((response: RestBranch[]) => {
        return response;
      })
    )
  }
  addRestBranch(model: any, dir: string) {
    return this.http.post(this.baseUrl + dir, model).pipe(
      map((branch: RestBranch) => {
        if (branch) {
          localStorage.setItem('branch', JSON.stringify(branch));
        }
        return branch;
      })
    )
  }
}
