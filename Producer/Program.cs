using System.Reflection;
using Confluent.Kafka;
using Contracts;
using Producer;
using Serilog;
using ServiceBus_Custom_Lib.Consumer;
using ServiceBus_Custom_Lib.Producer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()   
    .MinimumLevel.Debug()
    .CreateLogger();

builder.Services.AddSingleton<IServiceBus>(_ => new ServiceBus(new ProducerConfig
{
    BootstrapServers = "pkc-56d1g.eastus.azure.confluent.cloud:9092",
    SaslUsername = "OR666J4Z3XWDSSCV",
    SaslPassword = "s+YBx2NJ9aKXT3SNSeOHD1WcnRnFnfoY9lZRuKzTz/JVBwDW9v+pNn7+84ktK2mm",
    SecurityProtocol = SecurityProtocol.SaslSsl,
    SaslMechanism = SaslMechanism.Plain,
    Acks = Acks.Leader
}));

//Register consumers
builder.Services.AddTransient<OrderConsumer>();

var app = builder.Build();

//Register events (each event has 1 topic)
app.Services.SubscribeToTopic<OrderCreatedEvent>("order-created", Assembly.GetExecutingAssembly(), new ConsumerConfig()
{
    GroupId = AppDomain.CurrentDomain.FriendlyName,
    BootstrapServers = "pkc-56d1g.eastus.azure.confluent.cloud:9092",
    SaslUsername = "OR666J4Z3XWDSSCV",
    SaslPassword = "s+YBx2NJ9aKXT3SNSeOHD1WcnRnFnfoY9lZRuKzTz/JVBwDW9v+pNn7+84ktK2mm",
    SecurityProtocol = SecurityProtocol.SaslSsl,
    SaslMechanism = SaslMechanism.Plain,
    ClientId = AppDomain.CurrentDomain.FriendlyName,
    EnableAutoCommit = false,
    AutoOffsetReset = AutoOffsetReset.Earliest,
});

// builder.Services.AddSingleton<ConsumerConfig>(_ => new ConsumerConfig
// {
//     GroupId = AppDomain.CurrentDomain.FriendlyName,
//     BootstrapServers = "pkc-56d1g.eastus.azure.confluent.cloud:9092",
//     SaslUsername = "OR666J4Z3XWDSSCV",
//     SaslPassword = "s+YBx2NJ9aKXT3SNSeOHD1WcnRnFnfoY9lZRuKzTz/JVBwDW9v+pNn7+84ktK2mm",
//     SecurityProtocol = SecurityProtocol.SaslSsl,
//     SaslMechanism = SaslMechanism.Plain,
//     ClientId = AppDomain.CurrentDomain.FriendlyName,
//     // EnableAutoCommit = false,
//     AutoOffsetReset = AutoOffsetReset.Earliest,
// });

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
