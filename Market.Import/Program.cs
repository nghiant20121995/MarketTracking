using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Market.Infrastructure.MessageQueue;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Market.Domain.Interfaces.MessageQueue;
using Market.Domain.Interfaces.Provider;
using Market.Domain.Interfaces.Repository;
using Market.Domain.Interfaces.Services;
using Market.Infrastructure.Repository;
using Market.Service.Providers;
using Market.Service;

namespace Market.Import
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
            builder.Configuration.AddJsonFile($"{AppContext.BaseDirectory}/appsettings.json");
            builder.Services.AddScoped<IImportedService, ImportedFileService>();
            builder.Services.AddScoped<IFileService, CSVFileService>();
            builder.Services.AddScoped<IFileService, ExcelFileService>();
            builder.Services.AddScoped<IMarketPriceService, MarketPriceService>();
            builder.Services.AddScoped<IFileImportProvider, FileImportProvider>();
            builder.Services.AddScoped<IImportedFileRepository, ImportedFileRepository>();
            builder.Services.AddScoped<IKafkaProducer, KafkaProducer>();
            var connectionStr = builder.Configuration.GetConnectionString("Mongo");
            builder.Services.AddSingleton<IMongoClient, MongoClient>(e => new MongoClient(connectionStr));
            builder.Services.AddScoped((serviceProvider) =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                return client.GetDatabase("Market");
            });
            builder.Services.AddScoped<IMarketPriceTrackingRepository, MarketPriceTrackingRepository>();
            builder.Services.AddScoped((serviceProvider) =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                return client.GetDatabase("Market");
            });
            builder.Services.AddHostedService<MarketTrackingConsumer>();
            var app = builder.Build();
            app.Run();
        }
    }
}
