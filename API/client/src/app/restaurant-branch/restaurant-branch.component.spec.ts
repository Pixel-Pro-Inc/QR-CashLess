import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RestaurantBranchComponent } from './restaurant-branch.component';

describe('RestaurantBranchComponent', () => {
  let component: RestaurantBranchComponent;
  let fixture: ComponentFixture<RestaurantBranchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RestaurantBranchComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RestaurantBranchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
