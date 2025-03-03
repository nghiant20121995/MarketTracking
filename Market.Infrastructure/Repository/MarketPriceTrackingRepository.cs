using Market.Domain.Entities;
using Market.Domain.Interfaces.Repository;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Market.Domain.RequestModel;

namespace Market.Infrastructure.Repository
{
    public class MarketPriceTrackingRepository : BaseRepository<MarketPriceTracking>, IMarketPriceTrackingRepository
    {
        public MarketPriceTrackingRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public Task<List<MarketPriceTracking>> GetAllAsync(MarketPriceTrackingRequest request, CancellationToken cancellationToken = default)
        {
            int skip = (request.PageNumber - 1) * request.PageSize;
            return _collection
                .Find(_ => _.TransactionDate >= request.StartDate && _.TransactionDate <= request.EndDate)
                .Skip(skip).Limit(request.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
