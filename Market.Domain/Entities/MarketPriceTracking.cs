using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Entities
{
    public class MarketPriceTracking : BaseEntity
    {
        public DateTime TransactionDate { get; set; }
        public decimal Price { get; set; }
    }
}
