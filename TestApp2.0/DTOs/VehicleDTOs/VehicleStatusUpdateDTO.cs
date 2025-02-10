using System.ComponentModel.DataAnnotations;
using TestApp2._0.Models;

namespace TestApp2._0.DTOs.VehicleDTOs;

public class VehicleStatusUpdateDTO
{
    [Required(ErrorMessage = "VehicleId is Required")]
    public int VehicleId { get; set; }

    [Required]
    [EnumDataType(typeof(VehicleStatus), ErrorMessage = "Invalid Vehicle Status.")]
    public VehicleStatus VehicleStatus { get; set; }
}