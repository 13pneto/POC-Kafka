using Confluent.Kafka;

namespace Common;

public static class Configs
{
    public static ConsumerConfig ConsumerConfig(string groupId, string applicationName) => new ConsumerConfig
    {
        GroupId          =  groupId,
        BootstrapServers = "pkc-56d1g.eastus.azure.confluent.cloud:9092",
        SaslUsername = "OR666J4Z3XWDSSCV",
        SaslPassword = "s+YBx2NJ9aKXT3SNSeOHD1WcnRnFnfoY9lZRuKzTz/JVBwDW9v+pNn7+84ktK2mm",
        SecurityProtocol = SecurityProtocol.SaslSsl,
        SaslMechanism = SaslMechanism.Plain,
        ClientId = applicationName,
        // EnableAutoCommit = false,
        AutoOffsetReset = AutoOffsetReset.Earliest,
    };
}