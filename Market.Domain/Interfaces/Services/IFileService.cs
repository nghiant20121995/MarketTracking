using Market.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Market.Domain.Interfaces.Services
{
    public interface IFileService
    {
        Task ValidateAsync(IFormFile file);
        Task<ImportedFile> SaveFileAsync(IFormFile file);
        Task PublishMessageAsync(ImportedFile message);
        Task ProcessAsync(ImportedFile file);
    }
}
