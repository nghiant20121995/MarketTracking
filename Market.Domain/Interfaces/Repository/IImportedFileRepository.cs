using Market.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Domain.Interfaces.Repository
{
    public interface IImportedFileRepository
    {
        Task AddAsync(ImportedFile entity, CancellationToken cancellationToken = default);
        Task<ImportedFile> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<List<ImportedFile>> GetAllAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(ImportedFile entity, CancellationToken cancellationToken = default);
        List<ImportedFile> GetAll();
    }
}
