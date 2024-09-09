using Contracts;
using MassTransit;

namespace Producer;

public class OrderConsumer : IConsumer<OrderCreatedEvent>
{
    private int count = 1;

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        throw new Exception("TEST 123456");
        Console.WriteLine($"{count}: Received message, starting delay...");
        await Task.Delay(3000);
        Console.WriteLine($"{count}: Done");
        Interlocked.Increment(ref count);
    }
}