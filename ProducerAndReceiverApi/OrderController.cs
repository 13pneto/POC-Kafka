using Contracts;
using Microsoft.AspNetCore.Mvc;
using ServiceBus_Custom_Lib.Producer;

namespace Producer;

[ApiController]
public class OrderController : ControllerBase
{
    private readonly IServiceBus _serviceBus;

    public OrderController(IServiceBus serviceBus)
    {
        _serviceBus = serviceBus;
    }

    [HttpPost("custom")]
    public async Task<IActionResult> Custom()
    {
        var message = new OrderCreatedEvent();
        await _serviceBus.PublishAsync(message, "order-created");
        return Ok(message.Uuid);
    }
}