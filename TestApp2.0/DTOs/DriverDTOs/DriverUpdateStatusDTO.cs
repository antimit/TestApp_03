using System.ComponentModel.DataAnnotations;
using TestApp2._0.Models;

namespace TestApp2._0.DTOs.DriverDTOs;

public class DriverUpdateStatusDTO
{
    [Required(ErrorMessage = "DriverId is Required")]
    public int DriverId { get; set; }
    
    [Required]
    [EnumDataType(typeof(VehicleStatus), ErrorMessage = "Invalid Driver Status.")]
    public DriverStatus Status { get; set; }  
}

