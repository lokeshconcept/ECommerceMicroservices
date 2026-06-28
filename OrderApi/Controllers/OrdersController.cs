using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Models;
using System.Text.Json;

namespace OrderApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public OrdersController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderRequest request)
    {
        if (request == null)
            return BadRequest();

        var connectionString =
            _configuration["ServiceBus:ConnectionString"];

        var queueName =
            _configuration["ServiceBus:QueueName"];

        var order = new
        {
            OrderId = Guid.NewGuid(),
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        await using var client =
            new ServiceBusClient(connectionString);

        ServiceBusSender sender =
            client.CreateSender(queueName);

        string messageBody =
            JsonSerializer.Serialize(order);

        await sender.SendMessageAsync(
            new ServiceBusMessage(messageBody));

        return Ok(new
        {
            Message = "Order created and message sent to Service Bus",
            Order = order
        });
    }
}