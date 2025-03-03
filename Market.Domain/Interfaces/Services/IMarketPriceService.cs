using Market.Domain.Entities;
using Market.Domain.RequestModel;
using Market.Domain.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Interfaces.Services
{
    public interface IMarketPriceService
    {
        Task<MarketPriceTrackingResponse> GetAllAsync(MarketPriceTrackingRequest request);
        Task<decimal> GetMaxPriceAsync(MarketPriceTrackingRequest request);
        Task<decimal> GetMinPriceAsync(MarketPriceTrackingRequest request);
        Task<decimal> GetAveragePriceAsync(MarketPriceTrackingRequest request);
    }
}
