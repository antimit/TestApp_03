using System.ComponentModel.DataAnnotations;
using TestApp2._0.Models;

namespace TestApp2._0.DTOs.TransportationDTOs;

public class TransportationStatusUpdateDTO
{
    [Required(ErrorMessage = "TransportationId is Required")]
    public int StopId { get; set; }
    
    [Required]
    [EnumDataType(typeof(TransportationStatus), ErrorMessage = "Invalid Transportation Status.")]
    public TransportationStatus StopStatus { get; set; }
}