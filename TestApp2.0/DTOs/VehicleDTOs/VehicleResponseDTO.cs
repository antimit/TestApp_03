using TestApp2._0.Models;

namespace TestApp2._0.DTOs.VehicleDTOs;

public class VehicleResponseDTO
{
    public int VehicleId { get; set; }
    
   
    public string LicensePlate { get; set; }
    
    
    public string Make { get; set; }
    
    public string Model { get; set; }
    
    
    public string VIN { get; set; }
    
    
    public bool IsAvailable { get; set; } = true; 
    
    
    public VehicleStatus Status { get; set; }
    public List<Transportation> Transportations { get; set; } 
}