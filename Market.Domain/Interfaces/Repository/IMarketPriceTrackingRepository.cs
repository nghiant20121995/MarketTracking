using Market.Domain.Entities;
using Market.Domain.RequestModel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Domain.Interfaces.Repository
{
    public interface IMarketPriceTrackingRepository
    {
        Task AddAsync(MarketPriceTracking entity, CancellationToken cancellationToken = default);
        Task<MarketPriceTracking> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<List<MarketPriceTracking>> GetAllAsync(CancellationToken cancellationToken = default);
        List<MarketPriceTracking> GetAll();
        Task UpdateAsync(MarketPriceTracking entity, CancellationToken cancellationToken = default);
        Task<List<MarketPriceTracking>> GetAllAsync(MarketPriceTrackingRequest request,
            CancellationToken cancellationToken = default);
    }
}
