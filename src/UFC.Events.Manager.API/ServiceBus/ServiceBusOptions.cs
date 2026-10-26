using System.ComponentModel.DataAnnotations;

namespace UFC.Events.Manager.API.ServiceBus;

public sealed class ServiceBusOptions
{
    public const string SectionName = "ServiceBus";

    [Required]
    public string QueueName { get; init; } = string.Empty;

    [Range(1, 100)]
    public int MaxConcurrentCalls { get; init; } = 1;
}
