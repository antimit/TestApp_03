using System.Text.Json.Serialization;

namespace TestApp2._0.Models;


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StopStatus
{
    Pending = 1,     // Not yet arrived
    Arrived = 2,     // Truck has arrived but deliveries pending
    Completed = 3,   // All deliveries completed
    Skipped = 4      // Stop was skipped
}