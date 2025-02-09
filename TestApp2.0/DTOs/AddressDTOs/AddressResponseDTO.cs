namespace TestApp2._0.DTOs.AddressDTOs;

public class AddressResponseDTO
{
    public int AddressId { get; set; }

    
    public string Street { get; set; }

    public string City { get; set; }
    
    public string PostalCode { get; set; }

    public string Country { get; set; }

    public double? Latitude { get; set; }  
    public double? Longitude { get; set; } 
}