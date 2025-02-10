using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp2._0.Models;

public class Stop
{
    public int StopId { get; set; }


    public string StopOrder { get; set; }

    public double DistanceFromPreviousStop { get; set; }

    [Required] public int CustomerId { get; set; }

    [ForeignKey("CustomerId")] public Customer Customer { get; set; }

    [Required] public int AddressId { get; set; }
    [ForeignKey("AddressId")] public Address Address { get; set; }

    public ICollection<Delivery>? Deliveries { get; set; } = new List<Delivery>();

    [Required] public int? TransportationId { get; set; }
    [ForeignKey("TransportationId")] public Transportation? CurrentTransportation { get; set; }

    [Required] public StopStatus Status { get; set; } = StopStatus.Pending;
}