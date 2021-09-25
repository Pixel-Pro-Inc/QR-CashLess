import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any={} /** This is a class property to store what the user inputs into the form **/

  constructor() { }

  ngOnInit(): void {
  }

  login() {
    console.log(this.model);
  }
}
