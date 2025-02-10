using System.ComponentModel.DataAnnotations;

namespace TestApp2._0.Models;

public class Delivery
{
    public int DeliveryId { get; set; }

    public int? StopId { get; set; }
    public Stop? Stop { get; set; }

    public ICollection<DeliveryItem>? DeliveryItems { get; set; } = new List<DeliveryItem>();
    public DeliveryStatus Status { get; set; } = DeliveryStatus.Pending;

    public decimal TotalValue => DeliveryItems?.Sum(item => item.TotalCost) ?? 0;

    [Range(0, double.MaxValue)] public double TotalWeight => DeliveryItems?.Sum(item => item.ItemWeight) ?? 0;

    [Range(0, double.MaxValue)] public double TotalVolume => DeliveryItems?.Sum(item => item.ItemVolume) ?? 0;
}