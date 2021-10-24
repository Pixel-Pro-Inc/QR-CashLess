import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs/operators';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-photouploader',
  templateUrl: './photouploader.component.html',
  styleUrls: ['./photouploader.component.css']
})
export class PhotouploaderComponent implements OnInit {
  baseUrl = '';
  user: User;

  constructor(private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.baseUrl = this.accountService.baseUrl;
    this.initializeUploader();
  }

  initializeUploader() {

  }
}
