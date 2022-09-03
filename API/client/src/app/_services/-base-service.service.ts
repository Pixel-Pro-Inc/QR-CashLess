import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
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
  public baseUrl : string= 'https://localhost:5001/api'; 

}
