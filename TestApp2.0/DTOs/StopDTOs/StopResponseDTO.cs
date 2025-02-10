using TestApp2._0.Models;

namespace TestApp2._0.DTOs.StopDTOs;

public class StopResponseDTO
{
    public int StopId { get; set; }


    public string StopOrder { get; set; } = Guid.NewGuid().ToString();

    public double DistanceFromPreviousStop { get; set; }

    public int CustomerId { get; set; }


    public int AddressId { get; set; }

    public List<StopDeliveryResponse>? Deliveries { get; set; }

    public int? TransportationId { get; set; }

    public StopStatus Status { get; set; } = StopStatus.Pending;
}