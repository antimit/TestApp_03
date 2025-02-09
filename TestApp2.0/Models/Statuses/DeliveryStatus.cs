using System.Text.Json.Serialization;

namespace TestApp2._0.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum DeliveryStatus
{
    Pending = 1,      // Not yet delivered
    InTransit = 2,    // Currently on the way
    Delivered = 3,    // Successfully delivered
    Failed = 4,       // Delivery failed (customer unavailable, incorrect address, etc.)
    Canceled = 5      // Delivery was canceled
}