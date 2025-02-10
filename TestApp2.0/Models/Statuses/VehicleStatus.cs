using System.Text.Json.Serialization;

namespace TestApp2._0.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VehicleStatus
{
    Active = 1,
    UnderMaintenance = 2,
    Retired = 3,
    Reserved = 4
}