using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp2._0.Models;

public class DeliveryItem
{
    [Required] public int ProductId { get; set; }

    [ForeignKey("ProductId")] public Product Product { get; set; }

    public int DeliveryItemId { get; set; }
    public string Name { get; set; }

    public decimal SalesUnitPrice { get; set; }

    public int OrderedCount { get; set; }
    public int? DeliveredCount { get; set; }

    public int? CurrentDeliveryId { get; set; }
    public Delivery? CurrentDelivery { get; set; }

    public decimal TotalCost { get; set; }
    public int ItemWeight { get; set; }
    public int ItemVolume { get; set; }
}