﻿using Market.Domain.Entities;
using Market.Domain.Interfaces.Repository;
using Market.Domain.Interfaces.Services;
using Market.Domain.RequestModel;
using Market.Domain.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Market.Service
{
    public class MarketPriceService : IMarketPriceService
    {
        private readonly IMarketPriceTrackingRepository _marketPriceTrackingRepository;

        public MarketPriceService(IMarketPriceTrackingRepository marketPriceTrackingRepository)
        {
            _marketPriceTrackingRepository = marketPriceTrackingRepository;
        }
        public async Task<MarketPriceTrackingResponse> GetAllAsync(MarketPriceTrackingRequest request)
        {
            var rs = await _marketPriceTrackingRepository.GetAllAsync(request);
            return new MarketPriceTrackingResponse()
            {
                MarketPriceTrackings = rs
            };
        }

        public Task<decimal> GetMaxPriceAsync(MarketPriceTrackingRequest request)
        {
            return _marketPriceTrackingRepository.GetMaxPrice(request);
        }

        public Task<decimal> GetMinPriceAsync(MarketPriceTrackingRequest request)
        {
            return _marketPriceTrackingRepository.GetMinPrice(request);
        }

        public Task<decimal> GetAveragePriceAsync(MarketPriceTrackingRequest request)
        {
            return _marketPriceTrackingRepository.GetAveragePrice(request);
        }
    }
}
