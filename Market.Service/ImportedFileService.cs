using Market.Domain.Entities;
using Market.Domain.Interfaces.MessageQueue;
using Market.Domain.Interfaces.Repository;
using Market.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Service
{
    public class ImportedFileService : IImportedService
    {
        private readonly IImportedFileRepository _importedFileRepository;
        private readonly IConfiguration _configuration;

        public ImportedFileService(IImportedFileRepository importedFileRepository, IConfiguration configuration)
        {
            _importedFileRepository = importedFileRepository;
            _configuration = configuration;
        }

        public Task<ImportedFile> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return _importedFileRepository.GetByIdAsync(id);
        }
    }
}
