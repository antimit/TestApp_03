using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestApp2._0.Models;

namespace TestApp2._0.DTOs.DeliveryItemsDTOs;

public class DeliveryItemCreateDTO
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
    public string Name { get; set; }

    public int productId { get; set; }


    [Required(ErrorMessage = "Ordered Count is required.")]
    [Range(1, 1000, ErrorMessage = "Ordered Count must be between 1 and 1000.")]
    public int OrderedCount { get; set; }

    // [Range(0, 1000, ErrorMessage = "Delivered Count must be between 0 and 1000.")]
    // public int? DeliveredCount { get; set; }


    // [Required(ErrorMessage = "Delivery ID is required.")]
    // public int CurrentDeliveryId { get; set; }


    // [Required(ErrorMessage = "Total Cost is required.")]
    // [Column(TypeName = "decimal(18,2)")]
    // [Range(0.00, 1000000.00, ErrorMessage = "Total Cost must be between $0.00 and $1,000,000.00.")]
    // public decimal TotalCost { get; set; }
}