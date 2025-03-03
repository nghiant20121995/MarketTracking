using Market.Domain.Entities;
using Market.Domain.Interfaces.Repository;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Market.Domain.RequestModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace Market.Infrastructure.Repository
{
    public class MarketPriceTrackingRepository : BaseRepository<MarketPriceTracking>, IMarketPriceTrackingRepository
    {
        public MarketPriceTrackingRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public Task<List<MarketPriceTracking>> GetAllAsync(MarketPriceTrackingRequest? request, CancellationToken cancellationToken = default)
        {
            var pageNumber = request?.PageNumber > 0 ? request.PageNumber : 1;
            var pageSize = request?.PageSize > 0 ? request.PageSize : 30;
            int skip = (pageNumber - 1) * pageSize;
            var filter = Builders<MarketPriceTracking>.Filter.Empty;
            if (request?.StartDate != null) filter &= Builders<MarketPriceTracking>.Filter.Gte(_ => _.TransactionDate, request.StartDate);
            if (request?.EndDate != null) filter &= Builders<MarketPriceTracking>.Filter.Lte(_ => _.TransactionDate, request.EndDate);
            return _collection
                .Find(filter)
                .Skip(skip).Limit(pageSize)
                .ToListAsync(cancellationToken);
        }

        public Task<decimal> GetAveragePrice(MarketPriceTrackingRequest request)
        {
            return _collection
                    .Aggregate()
                    .Group(d => true, g => new { AvgPrice = g.Average(d => d.Price) })
                    .Project(d => d.AvgPrice)
                    .FirstOrDefaultAsync();
        }

        public Task<decimal> GetMaxPrice(MarketPriceTrackingRequest request)
        {
            return _collection
                .Find(_ => true)
                .SortByDescending(d => d.Price)
                .Limit(1)
                .Project(d => d.Price)
                .FirstOrDefaultAsync();
        }

        public Task<decimal> GetMinPrice(MarketPriceTrackingRequest request)
        {
            return _collection
                .Find(_ => true)
                .SortBy(d => d.Price)
                .Limit(1)
                .Project(d => d.Price)
                .FirstOrDefaultAsync();
        }
    }
}
