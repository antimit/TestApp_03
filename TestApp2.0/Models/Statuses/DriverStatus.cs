using System.Text.Json.Serialization;

namespace TestApp2._0.Models;


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DriverStatus
{
    Active = 1,    // Currently working
    OnLeave = 2,   // Temporarily unavailable
    Retired = 3,   // No longer working
    Suspended = 4  // License or company policy suspension
}