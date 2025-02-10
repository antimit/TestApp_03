using AutoMapper;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.AddressDTOs;
using TestApp2._0.DTOs.CustomerDTOs;
using TestApp2._0.DTOs.DeliveryDTOs;
using TestApp2._0.DTOs.DeliveryItemsDTOs;
using TestApp2._0.DTOs.DriverDTOs;
using TestApp2._0.DTOs.ProductDTOs;
using TestApp2._0.DTOs.StopDTOs;
using TestApp2._0.DTOs.TransportationDTOs;
using TestApp2._0.DTOs.VehicleDTOs;
using TestApp2._0.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StopCreateDTO, Stop>()
            .ForMember(dest => dest.StopId, opt => opt.Ignore()) 
            .ForMember(dest => dest.Deliveries, opt => opt.Ignore()); 

        CreateMap<Stop, StopResponseDTO>()
            .ForMember(dest => dest.Deliveries, opt => opt.MapFrom(src => src.Deliveries));

        CreateMap<Delivery, StopDeliveryResponse>();

        
        CreateMap<AddressAddDTO, Address>()
            .ForMember(dest => dest.AddressId, opt => opt.Ignore());

        CreateMap<Address, AddressResponseDTO>();
        
        

        CreateMap<CustomerAddDTO, Customer>()
            .ForMember(dest => dest.CustomerId, opt => opt.Ignore());
        
        CreateMap<Customer,CustomerResponseDTO>();
        
        
        

        CreateMap<DeliveryItemCreateDTO, DeliveryItem>()
            .ForMember(dest => dest.DeliveryItemId, opt => opt.Ignore());
        
        CreateMap< DeliveryItem,DeliveryItemResponseDTO>();
        
        
        
        CreateMap<DeliveryCreateDTO, Delivery>()
            .ForMember(dest => dest.DeliveryId, opt => opt.Ignore())
            .ForMember(dest => dest.DeliveryItems, opt => opt.Ignore()); 
        
        CreateMap<Delivery, DeliveryResponseDTO>()
            .ForMember(dest => dest.DeliveryItems, opt => opt.MapFrom(src => src.DeliveryItems));
        CreateMap<DeliveryItem, DeliveryDeliveryItemResponseDTO>();
        
        
        CreateMap<DriverAddDTO, Driver>()
            .ForMember(dest => dest.DriverId, opt => opt.Ignore());
        CreateMap<Driver, DriverResponseDTO>();
        
        
        
        CreateMap<ProductAddDTO, Product>()
            .ForMember(dest => dest.ProductId, opt => opt.Ignore());
        CreateMap<Product, ProductResponseDTO>();
        
        
        
        
        

        CreateMap<TransportationCreateDTO, Transportation>()
            .ForMember(dest => dest.TransportationId, opt => opt.Ignore())
            .ForMember(dest => dest.Stops, opt => opt.Ignore()); 
        
        CreateMap<Transportation, TransportationResponseDTO>()
            .ForMember(dest => dest.Stops, opt => opt.MapFrom(src => src.Stops));
        
        CreateMap<Stop, TransportationStopResponseDTO>();
        
        
        CreateMap<VehicleAddDTO, Vehicle>()
            .ForMember(dest => dest.VehicleId, opt => opt.Ignore());
        CreateMap<Vehicle, VehicleResponseDTO>();



    }
}