using Market.Domain.Entities;
using Market.Domain.Interfaces.Provider;
using Market.Domain.Interfaces.Services;
using Market.Domain.Response;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {

        private readonly ILogger<FileController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IFileImportProvider _fileImportProvider;
        private readonly IImportedService _importedService;

        public FileController(ILogger<FileController> logger, IConfiguration configuration, IFileImportProvider fileImportProvider, IImportedService importedService)
        {
            _logger = logger;
            _configuration = configuration;
            _fileImportProvider = fileImportProvider;
            _importedService = importedService;
        }

        [Route("importmarketprice")]
        [HttpPost]
        public async Task<ImportedFile> Post(IFormFile? file)
        {
            var fileService = _fileImportProvider.GetService((Path.GetExtension(file?.FileName) ?? string.Empty).TrimStart('.').ToLower());
            await fileService!.ValidateAsync(file!);
            var importedFile = await fileService.SaveFileAsync(file!);
            await fileService.PublishMessageAsync(importedFile);
            return importedFile;
        }
        [HttpGet]
        public async Task<ImportedFile> Get(string Id)
        {
            var rs = await _importedService.GetByIdAsync(Id);
            return rs;
        }
    }
}
