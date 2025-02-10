using System.ComponentModel.DataAnnotations;
using TestApp2._0.Models;

namespace TestApp2._0.DTOs.VehicleDTOs;

public class VehicleAddDTO
{
    [Required(ErrorMessage = "LicencePlate is required.")]
    [StringLength(10, MinimumLength = 5, ErrorMessage = "Licence Plate must be between 5 and 10 characters.")]
    public string LicensePlate { get; set; }


    [Required(ErrorMessage = "Vehicle Make is Required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Make must be at most 50 characters")]
    public string Make { get; set; }

    [Required(ErrorMessage = "Vehicle Model is Required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Model must be at most 50 characters")]
    public string Model { get; set; }


    [Required(ErrorMessage = "VIN is required.")]
    [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN must be exactly 17 characters.")]
    public string VIN { get; set; }
}