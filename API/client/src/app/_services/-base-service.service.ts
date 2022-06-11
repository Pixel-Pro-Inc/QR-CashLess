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
  // REFACTOR: Here is a good place to use an environment variable so we don't have future problems
  public baseUrl = 'https://rodizioexpress.com/api/'; 
  //public baseUrl = 'https://localhost:5001/api/';
}
