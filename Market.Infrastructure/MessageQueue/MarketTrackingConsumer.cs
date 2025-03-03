using Confluent.Kafka;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using System;
using Market.Domain.Interfaces.Services;
using Market.Domain.Entities;
using Market.Domain.Interfaces.Provider;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Market.Infrastructure.MessageQueue
{
    public class MarketTrackingConsumer : BackgroundService
    {
        private readonly ILogger<MarketTrackingConsumer> _logger;
        private readonly IMarketPriceService _marketPriceService;
        private readonly IFileImportProvider _fileImportProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        protected virtual string _topicName { get; set; }
        protected virtual int _consumerSize { get; set; }

        public MarketTrackingConsumer(IConfiguration configuration, 
            ILogger<MarketTrackingConsumer> logger, 
            IMarketPriceService marketPriceService,
            IFileImportProvider fileImportProvider,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _marketPriceService = marketPriceService;
            _fileImportProvider = fileImportProvider;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _topicName = _configuration.GetRequiredSection("Kafka").GetRequiredSection("Topic").GetRequiredSection("ImportFile").Value!;
            _logger = logger;
            _consumerSize = 2;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var listTask = new List<Task>();
            _logger.LogInformation($"----------------------------------------- start Subscribe {typeof(MarketTrackingConsumer).Name} -----------------------------------------------");
            for (var i = 0; i < _consumerSize; i++)
            {
                listTask.Add(Task.Factory.StartNew(async () =>
                {
                    await SubExecuteAsync(i, stoppingToken);
                }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Current));
            }
            await Task.WhenAll(listTask);
        }

        private async Task SubExecuteAsync(int indexConsumer, CancellationToken stoppingToken)
        {
            var bootstrapServers = _configuration
                .GetRequiredSection("Kafka")
                .GetRequiredSection("BootstrapServers").Value;
            var groupId = "market-import";
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                AllowAutoCreateTopics = true
            };
            var periodCommitNumber = 3;
            IConsumer<string, string> consumer = null;
            try
            {
                consumer = new ConsumerBuilder<string, string>(config).Build();
                consumer.Subscribe(_topicName);
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);
                        var importedFile = JsonSerializer.Deserialize<ImportedFile>(consumeResult.Message.Value);
                        using var newScope = _serviceProvider.CreateScope();
                        var fileService = _fileImportProvider.GetService((Path.GetExtension(importedFile?.Name) ?? string.Empty).TrimStart('.').ToLower());
                        await fileService!.ProcessAsync(importedFile!);
                        if (consumeResult.Offset % periodCommitNumber != 0) continue;
                        try
                        {
                            consumer.Commit(consumeResult);
                        }
                        catch (KafkaException e)
                        {
                            //GetMetaLogObject(logObject, originalMsg?.ListImpacts?.FirstOrDefault());
                            _logger.LogError($"Kafka Consume Commit error: {e.Error.Reason} | Topic: {_topicName} {indexConsumer}");

                            if (e.Error.Code == ErrorCode.IllegalGeneration)
                            {
                                _logger.LogInformation("Kafka", $"Resubscribe Topic: {_topicName} {indexConsumer}");
                                consumer.Subscribe(_topicName);
                            }
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Error while consuming Kafka message");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "consumer shut down");
                consumer?.Close();
                consumer?.Dispose();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
