using System.ComponentModel.DataAnnotations;

namespace TestApp2._0.DTOs.AddressDTOs;

public class AddressAddDTO
{
    [Required(ErrorMessage = "Street is required.")]
    [StringLength(100)]
    public string Street { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(50)]
    public string City { get; set; }

    [Required(ErrorMessage = "Postal Code is required.")]
    [StringLength(20)]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    [StringLength(50)]
    public string Country { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}