using System.Text.Json;
using Confluent.Kafka;
using Contracts;

namespace Common;

public class ConsumersRunnerBuilder
{
    public static string AppName { get; set; }
    public static CancellationToken CancellationToken { get; set; }
    private List<Task> Tasks { get; set; } = new();

    public ConsumersRunnerBuilder SetAppName(string appName)
    {
        AppName = appName;
        return this;
    }

    public ConsumersRunnerBuilder SetCancellationToken(CancellationToken ctsToken)
    {
        CancellationToken = ctsToken;
        return this;
    }

    public ConsumersRunnerBuilder SubscribeOrderCreated(string groupName)
    {
        StartConsumer(Constants.OrderCreatedTopic, groupName, nameof(OrderCreatedEvent));
        return this;
    }

    public ConsumersRunnerBuilder SubscribeOrderPaid(string groupName)
    {
        StartConsumer(Constants.OrderPaidTopic, groupName, nameof(OrderPaidEvent));
        return this;
    }

    public ConsumersRunnerBuilder SubscribeOrderShipped(string groupName)
    {
        StartConsumer(Constants.OrderShippedTopic, groupName, nameof(OrderShippedEvent));
        return this;
    }

    public ConsumersRunnerBuilder SubscribeOrderDelivered(string groupName)
    {
        StartConsumer(Constants.OrderDeliveredTopic, groupName, nameof(OrderDeliveredEvent));
        return this;
    }

    public async Task RunAsync()
    {
        await Task.WhenAll(Tasks);
    }

    private void StartConsumer(string topic, string groupName, string eventName)
    {
        var orderCreatedConsumer =
            new ConsumerBuilder<Null, string>(Configs.ConsumerConfig(groupName, AppName)).Build();

        orderCreatedConsumer.Subscribe(topic);

        var task = Task.Run(() =>
        {
            while (!CancellationToken.IsCancellationRequested)
            {
                var consumeResult = orderCreatedConsumer.Consume(CancellationToken);
                var deserializedEvent = JsonSerializer.Deserialize<BaseOrder>(consumeResult.Message.Value);
                Console.WriteLine(
                    $"Received {eventName}: {deserializedEvent.Uuid}. Partition: {consumeResult.Partition}. Offset: {consumeResult.Offset}");
            }
        }, CancellationToken);

        Tasks.Add(task);
    }
}