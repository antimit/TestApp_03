using TestApp2._0.Models;

namespace TestApp2._0.DTOs.DeliveryItemsDTOs;

public class DeliveryItemResponseDTO
{
    public int  DeliveryItemId { get; set; }
    
    public int productId { get; set; }
    
    public string Name { get; set; }
    
    public decimal SalesUnitPrice { get; set; }
    
    
    public int OrderedCount { get; set; }
    public int ?DeliveredCount { get; set; }
    
    public int ?CurrentDeliveryId { get; set; }
   
    
    public decimal TotalCost { get; set; } 
    public int ItemWeight { get; set; }
    public int ItemVolume { get; set; }
}
