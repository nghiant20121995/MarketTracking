using Market.Domain.Entities;
using Market.Domain.Interfaces.Repository;
using MongoDB.Driver;

namespace Market.Infrastructure.Repository
{
    public class ImportedFileRepository : BaseRepository<ImportedFile>, IImportedFileRepository
    {
        public ImportedFileRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }
    }
}
