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
            .ForMember(dest => dest.StopId, opt => opt.Ignore()) // Ignore ID since it's auto-generated
            .ForMember(dest => dest.Deliveries, opt => opt.Ignore()); // Handled separately

        CreateMap<Stop, StopResponseDTO>()
            .ForMember(dest => dest.Deliveries, opt => opt.MapFrom(src => src.Deliveries));

        CreateMap<Delivery, StopDeliveryResponse>();

        //Address
        CreateMap<AddressAddDTO, Address>()
            .ForMember(dest => dest.AddressId, opt => opt.Ignore());

        CreateMap<Address, AddressResponseDTO>();
        
        //Customer

        CreateMap<CustomerAddDTO, Customer>()
            .ForMember(dest => dest.CustomerId, opt => opt.Ignore());
        
        CreateMap<Customer,CustomerResponseDTO>();
        
        
        //DeliveryItem

        CreateMap<DeliveryItemCreateDTO, DeliveryItem>()
            .ForMember(dest => dest.DeliveryItemId, opt => opt.Ignore());
        
        CreateMap< DeliveryItem,DeliveryItemResponseDTO>();
        
        
        //Delivery
        CreateMap<DeliveryCreateDTO, Delivery>()
            .ForMember(dest => dest.DeliveryId, opt => opt.Ignore())
            .ForMember(dest => dest.DeliveryItems, opt => opt.Ignore()); // Handled separately
        
        CreateMap<Delivery, DeliveryResponseDTO>()
            .ForMember(dest => dest.DeliveryItems, opt => opt.MapFrom(src => src.DeliveryItems));
        CreateMap<DeliveryItem, DeliveryDeliveryItemResponseDTO>();
        
        //Driver
        CreateMap<DriverAddDTO, Driver>()
            .ForMember(dest => dest.DriverId, opt => opt.Ignore());
        CreateMap<Driver, DriverResponseDTO>();
        
        
        //Product
        CreateMap<ProductAddDTO, Product>()
            .ForMember(dest => dest.ProductId, opt => opt.Ignore());
        CreateMap<Product, ProductResponseDTO>();
        
        
        
        
        //Transportation

        CreateMap<TransportationCreateDTO, Transportation>()
            .ForMember(dest => dest.TransportationId, opt => opt.Ignore())
            .ForMember(dest => dest.Stops, opt => opt.Ignore()); // Handled separately
        
        CreateMap<Transportation, TransportationResponseDTO>()
            .ForMember(dest => dest.Stops, opt => opt.MapFrom(src => src.Stops));
        
        CreateMap<Stop, TransportationStopResponseDTO>();
        
        //Vehicle
        CreateMap<VehicleAddDTO, Vehicle>()
            .ForMember(dest => dest.VehicleId, opt => opt.Ignore());
        CreateMap<Vehicle, VehicleResponseDTO>();



    }
}