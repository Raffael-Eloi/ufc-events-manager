using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using UFC.Events.Manager.API.Entities;
using UFC.Events.Manager.API.Features.UFCEvents.EventsCoordinator;

namespace UFC.Events.Manager.API.ServiceBus;

public sealed class UfcEventsListener : BackgroundService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly ServiceBusClient _serviceBusClient;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ServiceBusOptions _options;
    private readonly ILogger<UfcEventsListener> _logger;

    public UfcEventsListener(
        ServiceBusClient serviceBusClient,
        IServiceScopeFactory scopeFactory,
        IOptions<ServiceBusOptions> options,
        ILogger<UfcEventsListener> logger)
    {
        _serviceBusClient = serviceBusClient;
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using ServiceBusProcessor processor = _serviceBusClient.CreateProcessor(
            _options.QueueName,
            new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = _options.MaxConcurrentCalls
            });

        processor.ProcessMessageAsync += HandleMessageAsync;
        processor.ProcessErrorAsync += HandleErrorAsync;

        await processor.StartProcessingAsync(stoppingToken);
        _logger.LogInformation("Listening to queue {QueueName}", _options.QueueName);

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            // Host is shutting down.
        }

        await processor.StopProcessingAsync(CancellationToken.None);
    }

    private async Task HandleMessageAsync(ProcessMessageEventArgs args)
    {
        List<UFCEvent>? events;
        try
        {
            events = args.Message.Body.ToObjectFromJson<List<UFCEvent>>(JsonOptions);
        }
        catch (JsonException exception)
        {
            _logger.LogError(exception, "Message {MessageId} has an invalid body", args.Message.MessageId);
            await args.DeadLetterMessageAsync(args.Message, "InvalidBody", exception.Message, args.CancellationToken);
            return;
        }

        if (events is null)
        {
            await args.DeadLetterMessageAsync(args.Message, "EmptyBody", "The message body is null", args.CancellationToken);
            return;
        }

        try
        {
            await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
            var coordinator = scope.ServiceProvider.GetRequiredService<IUfcEventsCoordinator>();
            await coordinator.ExecuteAsync(events);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            _logger.LogError(exception, "Failed to process message {MessageId}", args.Message.MessageId);
            await args.AbandonMessageAsync(args.Message, cancellationToken: args.CancellationToken);
            return;
        }

        await args.CompleteMessageAsync(args.Message, args.CancellationToken);
    }

    private Task HandleErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Service Bus error from {EntityPath} ({ErrorSource})", args.EntityPath, args.ErrorSource);
        return Task.CompletedTask;
    }
}
