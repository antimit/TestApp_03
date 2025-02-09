using System.Text.Json.Serialization;


namespace TestApp2._0.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransportationStatus
{
    Pending = 1,       // Shipment is created but not started
    InProgress = 2,    // Shipment is currently in transit
    Completed = 3,     // Shipment has successfully finished all stops
    Canceled = 4       // Shipment was canceled before completion
}



