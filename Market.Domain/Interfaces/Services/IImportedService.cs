using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Market.Domain.Interfaces.Services
{
    public interface IImportedService
    {
        Task<ImportedFile> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}
