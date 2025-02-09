using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp2._0.Models;

public class Vehicle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
    public int VehicleId { get; set; }
    
    public string LicensePlate { get; set; }
    
    
    public string Make { get; set; }
    
    public string Model { get; set; }
    
    
    public string VIN { get; set; }
    
    
    public bool IsAvailable { get; set; } = true; // Default to available
    
    public VehicleStatus Status { get; set; }
    public ICollection<Transportation> Transportations { get; set; } //Navigation Property
}