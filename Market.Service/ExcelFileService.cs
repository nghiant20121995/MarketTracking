using Market.Domain.Entities;
using Market.Domain.Enum;
using Market.Domain.Interfaces.MessageQueue;
using Market.Domain.Interfaces.Repository;
using Market.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Market.Service
{
    public class ExcelFileService : IFileService
    {
        private readonly IImportedFileRepository _importedFileRepository;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly IConfiguration _configuration;

        public ExcelFileService(IImportedFileRepository importedFileRepository, IKafkaProducer kafkaProducer, IConfiguration configuration)
        {
            _importedFileRepository = importedFileRepository;
            _kafkaProducer = kafkaProducer;
            _configuration = configuration;
        }

        public Task PublishMessageAsync(ImportedFile message)
        {
            var topic = _configuration
                .GetRequiredSection("Kafka")
                .GetRequiredSection("Topic")
                .GetRequiredSection("ImportFile").Value;
            return _kafkaProducer.ProduceAsync(topic!, message.Id, message);
        }

        public async Task<ImportedFile> SaveFileAsync(IFormFile file)
        {
            var result = await StoreFileAsync(file);
            var newImportedFile = new ImportedFile()
            {
                Id = result.Item1,
                Name = result.Item2,
                FileType = (int)ImportedFileType.Excel,
                IsProcessed = false,
                Path = result.Item3,
                Url = result.Item3,
            };
            await _importedFileRepository.AddAsync(newImportedFile);
            return newImportedFile;
        }

        protected async Task<(string, string, string)> StoreFileAsync(IFormFile file)
        {
            var newFileId = Guid.NewGuid().ToString();
            var newFileName = newFileId + Path.GetExtension(file.FileName);

            string uploadPath = Path.Combine("\\uploads\\excel\\");

            // Ensure the directory exists
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string filePath = Path.Combine(uploadPath, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return (newFileId, newFileName, filePath);
        }

        public Task ValidateAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public Task ProcessAsync(ImportedFile file)
        {
            throw new NotImplementedException();
        }
    }
}
