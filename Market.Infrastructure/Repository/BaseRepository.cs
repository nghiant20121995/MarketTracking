using Market.Domain.Entities;
using Market.Domain.Interfaces.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Infrastructure.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> _collection;
        public BaseRepository(IMongoDatabase mongoDatabase) {
            string collectionName = typeof(T).Name;
            _collection = mongoDatabase.GetCollection<T>(collectionName);
        }
        public virtual Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(entity.Id)) entity.Id = Guid.NewGuid().ToString();
            entity.CreatedDate = DateTime.UtcNow;
            return _collection.InsertOneAsync(entity, null, cancellationToken);
        }

        public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);
            return _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
        }

        public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            return _collection.DeleteOneAsync(e => e.Id.Equals(entity.Id), cancellationToken);
        }

        public virtual List<T> GetAll()
        {
            return _collection.Find(e => true).ToList();
        }

        public virtual Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _collection.Find(_ => true).ToListAsync(cancellationToken);
        }

        public virtual Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return _collection.Find(e => e.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
