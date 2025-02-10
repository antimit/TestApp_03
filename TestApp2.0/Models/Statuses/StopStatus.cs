using System.Text.Json.Serialization;

namespace TestApp2._0.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StopStatus
{
    Pending = 1,
    Arrived = 2,
    Completed = 3,
    Skipped = 4
}