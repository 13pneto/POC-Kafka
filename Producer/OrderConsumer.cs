using Contracts;
using Serilog;
using ServiceBus_Custom_Lib.Consumer;

namespace Producer;

public class OrderConsumer : IConsume<OrderCreatedEvent>
{
    //Custom
    public Task HandleAsync(OrderCreatedEvent message)
    {
        // throw new Exception("Test exception");
        Log.Information($"(Custom) Message of type {typeof(OrderCreatedEvent)} received. Uuid: {message.Uuid}, Date: {message.Date}");
        return Task.CompletedTask;
    }
}