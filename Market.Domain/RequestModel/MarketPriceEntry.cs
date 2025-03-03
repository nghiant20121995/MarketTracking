using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.RequestModel
{
    public class MarketPriceEntry
    {
        public MarketPriceTracking? MarketPriceTracking { get; set; }
        public string? Input {  get; set; }
    }
}
