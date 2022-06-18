import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, NavigationStart, Router } from '@angular/router';
import { Local } from 'protractor/built/driverProviders';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';
import { ReferenceService } from './_services/reference.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Rodizio Express';
  users: any;
  showViewOrder;

  constructor(private accountService: AccountService, public referenceService: ReferenceService, private route: ActivatedRoute, router:Router) {
    router.events.forEach((event) => {
      if(event instanceof NavigationEnd) {
        if(router.url.includes('menu') && !router.url.includes('edit')){
          this.showViewOrder = true;
        }
        else{
          this.showViewOrder = false;
        }
      }
    });
  }
  SelectorMode: boolean;



  ngOnInit() {
    this.setCurrentUser();
    console.log(this.referenceService.currentreference());
    this.SelectorMode = true;
  }

  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
  }

  navigateToSite(link: string){
    window.open(link);
  }

  RemoveSelector(event: boolean) {

    this.SelectorMode = event;
  }
}
