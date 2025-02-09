using System.ComponentModel.DataAnnotations;

namespace TestApp2._0.DTOs.TransportationDTOs;

public class TransportationStopCreateDTO
{
    [Required(ErrorMessage = "Stop ID is required.")]
    public int StopId { get; set; }
    
   
}