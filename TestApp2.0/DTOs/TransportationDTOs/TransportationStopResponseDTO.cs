using TestApp2._0.Models;

namespace TestApp2._0.DTOs.TransportationDTOs;

public class TransportationStopResponseDTO
{

    public string StopOrder { get; set; }

    public double DistanceFromPreviousStop { get; set; }

    public int CustomerId { get; set; }

   

    public int AddressId { get; set; }
    
    public StopStatus Status { get; set; } 
    
}