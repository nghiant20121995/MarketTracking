using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.RequestModel
{
    public class MarketPriceTrackingRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
