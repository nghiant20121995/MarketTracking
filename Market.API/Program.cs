using Market.Domain.Interfaces.MessageQueue;
using Market.Domain.Interfaces.Provider;
using Market.Domain.Interfaces.Repository;
using Market.Domain.Interfaces.Services;
using Market.Infrastructure.MessageQueue;
using Market.Infrastructure.Repository;
using Market.Service;
using Market.Service.Extensions;
using Market.Service.Providers;
using MongoDB.Driver;
using Serilog;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
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
            builder.Services.AddControllers();

            Log.Logger = new LoggerConfiguration()
                    .WriteTo.File($"{AppContext.BaseDirectory}/logs/log_.txt", rollingInterval: RollingInterval.Day) // Daily log file rotation
                    .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddCors((options) =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()         // Allow all origins
                          .AllowAnyMethod()         // Allow all HTTP methods (GET, POST, etc.)
                          .AllowAnyHeader();        // Allow all headers
                });
            });

            var app = builder.Build();

            app.UseCors("AllowAll");

            app.UseExceptionHandler(builder =>
            {
                builder.UseGlobalExceptionProcess();
            });

            app.MapControllers();


            app.Run();
        }
    }
}
