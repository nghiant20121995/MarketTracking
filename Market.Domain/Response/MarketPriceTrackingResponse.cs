using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Response
{
    public class MarketPriceTrackingResponse
    {
        public int TotalRecord { get; set; }
        public IEnumerable<MarketPriceTracking>? MarketPriceTrackings { get; set; }
        public int TotalPage { get; set; }
    }

    public class DashboardTrackingResponse
    {
        public decimal MaximumPrice { get; set; }
        public decimal MinimumPrice { get; set; }
        public decimal Average { get; set; }
    }
}
