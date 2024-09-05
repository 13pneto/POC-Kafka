using Confluent.Kafka;

namespace Common;

public static class Constants
{
    public static string OrderCreatedTopic = "order-created";
    public static string OrderPaidTopic = "order-paid";
    public static string OrderShippedTopic = "order-shipped";
    public static string OrderDeliveredTopic = "order-delivered";
}