using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OrderWorker.Data;
using OrderWorker.Models;
using System.Text.Json;

namespace OrderWorker;

public class ProcessOrderFunction
{
    private readonly ILogger<ProcessOrderFunction> _logger;
    private readonly OrderDbContext _dbContext;

    public ProcessOrderFunction(ILogger<ProcessOrderFunction> logger, OrderDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [Function(nameof(ProcessOrderFunction))]
    public async Task Run(
        [ServiceBusTrigger("orders", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        var body = message.Body.ToString();
        _logger.LogInformation("Order received: {Message}", message);

        var orderMessage =
            JsonSerializer.Deserialize<OrderMessage>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        if (orderMessage == null)
        {
            _logger.LogError("Invalid order message.");
            return;
        }

        var order = new Order
        {
            Id = orderMessage.OrderId,
            ProductId = orderMessage.ProductId,
            Quantity = orderMessage.Quantity,
            CreatedAt = orderMessage.CreatedAt
        };

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Order inserted into database: {OrderId}", order.Id);


        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}

public class OrderMessage
{
    public Guid OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
}