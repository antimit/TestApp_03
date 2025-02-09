using System.Text.Json.Serialization;

namespace TestApp2._0.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VehicleStatus
{
    Active = 1,          // Available for assignments
    UnderMaintenance = 2, // In repair
    Retired = 3,         // No longer in service
    Reserved = 4         // Reserved for an upcoming transportation
}