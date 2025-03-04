using Market.Domain.Entities;
using Market.Domain.Interfaces.Repository;
using Market.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Market.Domain.Interfaces.MessageQueue;
using Market.Domain.Enum;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Collections.Generic;
using Market.Domain.RequestModel;

namespace Market.Service
{
    public class CSVFileService : IFileService
    {
        private readonly IImportedFileRepository _importedFileRepository;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly IConfiguration _configuration;
        private readonly IMarketPriceTrackingRepository _marketPriceTrackingRepository;

        private readonly Dictionary<string, Func<MarketPriceEntry, bool>> _funcMapper = new Dictionary<string, Func<MarketPriceEntry, bool>>()
        {
            ["date"] = ProcessTransactionTime,
            ["market price ex1"] = ProcessMarketPrice,
        };

        public CSVFileService(IImportedFileRepository importedFileRepository, IKafkaProducer kafkaProducer, IConfiguration configuration,
            IMarketPriceTrackingRepository marketPriceTrackingRepository)
        {
            _importedFileRepository = importedFileRepository;
            _kafkaProducer = kafkaProducer;
            _configuration = configuration;
            _marketPriceTrackingRepository = marketPriceTrackingRepository;
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
                FileType = (int)ImportedFileType.Csv,
                IsProcessed = false,
                Path = result.Item3,
                Url = result.Item3,
                ExpiredDate = DateTime.UtcNow.AddMinutes(5),
            };
            await _importedFileRepository.AddAsync(newImportedFile);
            return newImportedFile;
        }

        protected async Task<(string, string, string)> StoreFileAsync(IFormFile file)
        {
            var newFileId = Guid.NewGuid().ToString();
            var newFileName = newFileId + ".csv";

            string uploadPath = Path.Combine("/app/market/api/uploads/csv/");

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



        public async Task ValidateAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) throw new ArgumentNullException("The CSV file is empty");
            return;
        }

        public async Task ProcessAsync(ImportedFile file)
        {
            var path = "/app/market/import/uploads/csv/" + file.Name;
            if (!File.Exists(path))
            {
                file.ErrorMessage = $"The CSV file is not found. File name: {file.Name}";
                file.IsProcessed = true;
                await _importedFileRepository.UpdateAsync(file);
                return;
            }
            using var reader = new StreamReader(path);
            var headerLine = await reader.ReadLineAsync();
            if (headerLine == null)
            {
                file.ErrorMessage = "The CSV file is empty.";
                file.IsProcessed = true;
                await _importedFileRepository.UpdateAsync(file);
                return;
            }

            var headers = headerLine.Split(',');
            var processorMap = CreateFuncMap(headers);

            var listMarketPrice = new List<MarketPriceTracking>();

            while (!reader.EndOfStream)
            {
                var newMarketPriceEntry = new MarketPriceEntry()
                {
                    MarketPriceTracking = new MarketPriceTracking()
                };
                var newMarketPrice = new MarketPriceTracking();
                var line = await reader.ReadLineAsync();
                var values = line.Split(',');
                var processResult = false;
                for (int i = 0; i < values.Length; i++)
                {
                    newMarketPriceEntry.Input = values[i];
                    processResult = processorMap[i].Invoke(newMarketPriceEntry);
                }
                if (processResult)
                {
                    await _marketPriceTrackingRepository.AddAsync(newMarketPriceEntry.MarketPriceTracking);
                }
            }

            file.IsProcessed = true;
            await _importedFileRepository.UpdateAsync(file);
        }

        private Dictionary<int, Func<MarketPriceEntry, bool>> CreateFuncMap(string[] headers)
        {
            var newActionMap = new Dictionary<int, Func<MarketPriceEntry, bool>>();
            for (int i = 0; i < headers.Length; i++)
            {
                if (_funcMapper.TryGetValue(headers[i].ToLower(), out var action))
                {
                    newActionMap.Add(i, action);
                }
            }
            return newActionMap;
        }

        private static bool ProcessTransactionTime(MarketPriceEntry entry)
        {
            if (entry.MarketPriceTracking == null) return false;
            var dateFormats = new[]
            {
                "dd/MM/yyyy HH:mm", // Format for "13/01/2017 00:30"
                "dd/MM/yyyy",       // Format for "13/01/2017"
                "d/M/yyyy HH:mm",   // Format for "10/1/2017 0:30" (single-digit day/month and hour)
                "d/M/yyyy",         // Format for "10/1/2017" (single-digit day/month)
                "MM/dd/yyyy HH:mm", // Format for "01/10/2017 00:30" (if needed)
                "MM/dd/yyyy",       // Format for "01/10/2017" (if needed)
                "yyyy-MM-dd HH:mm", // Format for "2017-01-10 00:30" (ISO format)
                "yyyy-MM-dd"        // Format for "2017-01-10" (ISO format)
            };
            var culture = CultureInfo.InvariantCulture;
            var dateTimeStyle = DateTimeStyles.None;

            // Try parsing the date-time value with multiple formats
            if (DateTime.TryParseExact(entry.Input, dateFormats, culture, dateTimeStyle, out var dateValue))
            {
                entry.MarketPriceTracking.TransactionDate = dateValue;
                return true;
            }
            if (DateTime.TryParse(entry.Input, culture, dateTimeStyle, out var fallbackDateValue))
            {
                entry.MarketPriceTracking.TransactionDate = fallbackDateValue;
                return true;
            }
            return false;
        }

        private static bool ProcessMarketPrice(MarketPriceEntry entry)
        {
            if (entry.MarketPriceTracking == null) return false;
            if (decimal.TryParse(entry.Input, out var decimalValue))
            {
                entry.MarketPriceTracking.Price = decimalValue; // Store as decimal
                return true;
            }
            return false;
        }
    }
}
