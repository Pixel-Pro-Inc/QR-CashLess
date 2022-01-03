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
  /**
   * so the set reference is used in Menu component line:59
   * @param param
   * are set in restaurant Branch onClick()
   */
  setReference(param: string) {
    console.log("This is the params " + param);

    let indexOfBreak = param.indexOf('_');
    let ref = param.slice(indexOfBreak + 1, param.length);
    console.log("This is the reference " + ref);

    let branch = param.slice(0, indexOfBreak);
    console.log("This is the branch " + branch);

    localStorage.setItem('reference', ref);

    this.setBranch(branch);
    
  }

  setBranch(branch: string){
    localStorage.setItem('branch', branch);
  }
}
