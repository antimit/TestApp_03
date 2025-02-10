using System.Text.Json.Serialization;


namespace TestApp2._0.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransportationStatus
{
    Pending = 1,
    InProgress = 2,
    Completed = 3,
    Canceled = 4
}