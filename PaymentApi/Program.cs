using Common;

const string orderPaidGroupName = "PaymentApi-OrderPaid";

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
        .SubscribeOrderPaid(orderPaidGroupName)
        .RunAsync();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
