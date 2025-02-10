using System.Text.Json.Serialization;

namespace TestApp2._0.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeliveryStatus
{
    Pending = 1,
    InTransit = 2,
    Delivered = 3,
    Failed = 4,
    Canceled = 5
}