using Confluent.Kafka;
using Contracts;
using MassTransit;
using Producer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()   
    .MinimumLevel.Debug()
    .CreateLogger();

builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory();

    x.AddRider(rider =>
    {
        rider.AddConsumer<OrderConsumer>();

        rider.UsingKafka((context, k) =>
        {
            k.Host("pkc-56d1g.eastus.azure.confluent.cloud:9092", x =>
            {
                x.UseSasl(authConfig =>
                {
                    authConfig.Username = "OR666J4Z3XWDSSCV";
                    authConfig.Password = "s+YBx2NJ9aKXT3SNSeOHD1WcnRnFnfoY9lZRuKzTz/JVBwDW9v+pNn7+84ktK2mm";
                    authConfig.Mechanism = SaslMechanism.Plain;
                    authConfig.SecurityProtocol = SecurityProtocol.SaslSsl;
                });
            });

            k.TopicEndpoint<OrderCreatedEvent>("topic_8", "XXXXXXXXXXXX2", e =>
            {
                e.AutoOffsetReset = AutoOffsetReset.Earliest;
                e.ConcurrentDeliveryLimit = 5;
                e.ConfigureConsumer<OrderConsumer>(context);
            });
        });
    });
});

var app = builder.Build();



//Register events (each event has 1 topic)

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
