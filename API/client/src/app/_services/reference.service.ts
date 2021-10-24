import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ReferenceService {
  currentreference: string;
  constructor() { }

  setReference(ref: string) {
    localStorage.setItem('reference', ref);
    this.currentreference = localStorage.getItem('reference');
  }
}
