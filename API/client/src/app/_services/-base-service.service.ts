import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusyService } from './busy.service';

@Injectable({
  providedIn: 'root'
})
export class BaseServiceService {

  constructor(protected http: HttpClient) { }

  /**
   *this needs to be public so anything that uses it, eg components, will take from it. Im confident its safe cause all the services are private properties injected into
   * components anyways
   */
  //public baseUrl = 'https://rodizioexpress.azurewebsites.net/api/'; 
  public baseUrl = 'https://localhost:5001/api/'; 
}
