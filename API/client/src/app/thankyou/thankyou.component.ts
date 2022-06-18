import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { timer } from 'rxjs';
import { ReferenceService } from '../_services/reference.service';

@Component({
  selector: 'app-thankyou',
  templateUrl: './thankyou.component.html',
  styleUrls: ['./thankyou.component.css']
})
export class ThankyouComponent implements OnInit {

  constructor(private router: Router, private referenceService: ReferenceService) { }

  ngOnInit(): void {
    this.init();
  }

  async init() {
    await this.sleep(10000);

    this.router.navigateByUrl('/menu/' + this.referenceService.currentBranch() + '_' + this.referenceService.currentreference());
  }
  
  sleep(ms) {
    return new Promise((resolve) => {
      setTimeout(resolve, ms);
    });
  }

  navigateToSite(link: string){
    window.open(link);
  }
  
}
