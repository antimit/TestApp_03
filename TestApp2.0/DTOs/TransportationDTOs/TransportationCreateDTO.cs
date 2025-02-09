using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestApp2._0.Models;

namespace TestApp2._0.DTOs.TransportationDTOs;

public class TransportationCreateDTO
{
   
    
    public int DriverId { get; set; }
    
    public int TruckId { get; set; }
    
    
    public List<TransportationStopCreateDTO> Stops { get; set; }
    public TransportationStatus Status { get; set; }
    
    //Dates
    
    [Required(ErrorMessage = "Departure Time is required")]
    [DataType(DataType.DateTime)]
    public DateTime  CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    [Required]
    public DateTime? DepartureTime { get; set; }
    
    public DateTime? ActualArrivalTime { get; set; }


    public int? LastStopId { get; set; }
    
    [Range (0, int.MaxValue, ErrorMessage = "Completed stops cannot be negative")]
    public int CompletedStopsCount { get; set; }
    
    public bool IsFinalized { get; set; }
}