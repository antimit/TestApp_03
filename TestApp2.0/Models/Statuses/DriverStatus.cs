using System.Text.Json.Serialization;

namespace TestApp2._0.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DriverStatus
{
    Active = 1,
    OnLeave = 2,
    Retired = 3,
    Suspended = 4
}