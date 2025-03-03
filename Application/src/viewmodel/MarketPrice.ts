export class MarketPrice {
    total: number;
    totalPage: number;
    marketPriceTrackings: Array<MarketPriceTracking>;
}

export class MarketPriceTracking {
    id: number;
    transactionDate: Date;
    price: number;
}

export class MarketPriceTrackingRequest {
    startDate?: Date;
    endDate?: Date;
    pageNumber: number;
    pageSize: number;
  
    constructor(pageNumber: number, pageSize: number, startDate?: Date, endDate?: Date) {
      this.startDate = startDate;
      this.endDate = endDate;
      this.pageNumber = pageNumber;
      this.pageSize = pageSize;
    }
  }
  