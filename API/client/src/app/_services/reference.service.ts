import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ReferenceService {
  branchId: string;
  constructor() { }

  currentreference(): string{
    return localStorage.getItem('reference');
  }

  currentBranch(): string{
    return localStorage.getItem('branch');
  }

  setReference(param: string) {
    let indexOfBreak = param.indexOf('_');

    let ref = param.slice(indexOfBreak + 1, param.length);

    let branch = param.slice(0, indexOfBreak);
    
    localStorage.setItem('reference', ref);

    this.setBranch(branch);
    
  }

  setBranch(branch: string){
    localStorage.setItem('branch', branch);
  }
}
