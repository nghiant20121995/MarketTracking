import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MarketpriceComponent } from './marketprice.component';

describe('MarketpriceComponent', () => {
  let component: MarketpriceComponent;
  let fixture: ComponentFixture<MarketpriceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MarketpriceComponent]
    });
    fixture = TestBed.createComponent(MarketpriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
