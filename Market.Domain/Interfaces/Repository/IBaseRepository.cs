using Market.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Domain.Interfaces.Repository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        List<T> GetAll();
    }
}
