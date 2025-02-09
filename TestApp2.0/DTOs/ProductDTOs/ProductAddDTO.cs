using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp2._0.DTOs.ProductDTOs;

public class ProductAddDTO
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Sales Unit Price is required.")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, 100000.00, ErrorMessage = "Price must be between $0.01 and $100,000.00.")]
    public decimal SalesUnitPrice { get; set; }

    [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Weight is required.")]
    [Range(0, 1000, ErrorMessage = "Weight must be between 0 and 1000 kg.")]
    public int Weight { get; set; }

    [Required(ErrorMessage = "Volume is required.")]
    [Range(0, 1000, ErrorMessage = "Volume must be between 0 and 1000 cubic meters.")]
    public int Volume { get; set; }
}