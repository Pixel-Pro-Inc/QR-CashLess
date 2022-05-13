import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  // OBSOLETE: We need to remove these, they have no use
  constructor() { }

  ngOnInit(): void {
  }

}
