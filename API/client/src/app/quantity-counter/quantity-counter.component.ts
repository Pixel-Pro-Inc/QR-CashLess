import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-quantity-counter',
  templateUrl: './quantity-counter.component.html',
  styleUrls: ['./quantity-counter.component.css']
})
export class QuantityCounterComponent implements OnInit {
  @Input() model:any = {};

  constructor() { }

  ngOnInit(): void {
    this.model.quantity = 1;
  }

  minus(){
    if(this.model.quantity != 1){
      this.model.quantity--;
    }
  }

  plus(){
    this.model.quantity++;

    this.display();
  }

  display(){
    console.log(this.model);
  }

}
