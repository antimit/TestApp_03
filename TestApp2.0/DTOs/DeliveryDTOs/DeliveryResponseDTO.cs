using System.ComponentModel.DataAnnotations;
using TestApp2._0.Models;

namespace TestApp2._0.DTOs.DeliveryDTOs;

public class DeliveryResponseDTO
{
    public int DeliveryId { get; set; }
    
    public int? StopId { get; set; } 

    public List<DeliveryDeliveryItemResponseDTO>? DeliveryItems { get; set; }

    [Required] 
    public DeliveryStatus Status { get; set; } = DeliveryStatus.Pending;
    
    // Cost & Pricing Data
    public decimal TotalValue => DeliveryItems?.Sum(item => item.TotalCost) ?? 0;

    [Range(0, double.MaxValue)]
    public double TotalWeight => DeliveryItems?.Sum(item => item.ItemWeight) ?? 0;

    [Range(0, double.MaxValue)]
    public double TotalVolume => DeliveryItems?.Sum(item => item.ItemVolume) ?? 0;
}