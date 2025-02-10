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

    
    


    
    


    
    
    
    
}