using Common;

const string orderCreatedGroupName = "NotificationApi-OrderCreated";
const string orderPaidGroupName = "NotificationApi-OrderPaid";
const string orderShippedGroupName = "NotificationApi-OrderShipped";
const string orderDeliveredGroupName = "NotificationApi-OrderDelivered";

var appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true; // Prevent the process from terminating immediately.
    cts.Cancel(); // Request cancellation for the tasks.
};

try
{
    await new ConsumersRunnerBuilder()
        .SetAppName(appName)
        .SetCancellationToken(cts.Token)
        .SubscribeOrderCreated(orderCreatedGroupName)
        .SubscribeOrderPaid(orderPaidGroupName)
        .SubscribeOrderShipped(orderShippedGroupName)
        .SubscribeOrderDelivered(orderDeliveredGroupName)
        .RunAsync();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
