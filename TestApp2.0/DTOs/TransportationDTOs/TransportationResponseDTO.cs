using System.ComponentModel.DataAnnotations;
using TestApp2._0.Models;

namespace TestApp2._0.DTOs.TransportationDTOs;

public class TransportationResponseDTO
{
    public int TransportationId { get; set; }

    public int DriverId { get; set; }
    public Driver Driver { get; set; }


    public int TruckId { get; set; }
    public Vehicle Truck { get; set; }


    public ICollection<Stop> Stops { get; set; }
    public TransportationStatus Status { get; set; }


    [Required(ErrorMessage = "Departure Time is required")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    [Required] public DateTime? DepartureTime { get; set; }

    public DateTime? ActualArrivalTime { get; set; }


    public int? LastStopId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Completed stops cannot be negative")]
    public int CompletedStopsCount { get; set; }

    public bool IsFinalized { get; set; }
}