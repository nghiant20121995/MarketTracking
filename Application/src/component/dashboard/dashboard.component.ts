import { Component, OnDestroy, OnInit } from '@angular/core';
import MarketPriceTrackingService from '../../dataservice/MarketTrackingService';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {
  MaxPrice: number;
  MinPrice: number;
  AveragePrice: number;

  constructor(private readonly marketPriceTrackingService: MarketPriceTrackingService) {

  }
  ngOnInit(): void {
    this.marketPriceTrackingService.GetMaxPrice().subscribe((response) => {
      this.MaxPrice = response;
    });
    this.marketPriceTrackingService.GetMinPrice().subscribe((response) => {
      this.MinPrice = response;
    });
    this.marketPriceTrackingService.GetMinPrice().subscribe((response) => {
      this.AveragePrice = response;
    });
  }

  ngOnDestroy(): void {
  }
}
