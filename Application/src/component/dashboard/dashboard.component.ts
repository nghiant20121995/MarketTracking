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
    this.marketPriceTrackingService.GetDashboardFigure().subscribe((response) => {
      this.MaxPrice = response.MaximumPrice;
      this.MinPrice = response.MinimumPrice;
      this.AveragePrice = response.AveragePrice;
    });
  }

  ngOnDestroy(): void {
  }
}
