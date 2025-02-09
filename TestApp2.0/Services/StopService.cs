using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.DeliveryDTOs;
using TestApp2._0.DTOs.StopDTOs;
using TestApp2._0.Models;

namespace TestApp2._0.Services;

public class StopService
{
    private readonly ApplicationDbContext _context;
    
    private static readonly Dictionary<StopStatus, List<StopStatus>> AllowedStatusTransitions = new()
    {
        { StopStatus.Pending, new List<StopStatus> { StopStatus.Arrived, StopStatus.Skipped } },
        { StopStatus.Arrived, new List<StopStatus> { StopStatus.Completed, StopStatus.Skipped } },
        { StopStatus.Completed, new List<StopStatus>() }, 
        { StopStatus.Skipped, new List<StopStatus>() }    
    };

    public StopService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<ApiResponse<StopResponseDTO>> CreateStopAsync(StopCreateDTO stopDto)
        {
            try
            {
                var errors = new List<string>();
                
                var address = await _context.Addresses.FindAsync(stopDto.AddressId);
                if (address == null)
                {
                    errors.Add($"Address with ID {stopDto.AddressId} does not exist.");
                }
                
                var customer = await _context.Customers.FindAsync(stopDto.CustomerId);
                if (customer == null)
                {
                    errors.Add($"Customer with ID {stopDto.CustomerId} does not exist.");
                }
    
                Transportation? transportation = null;
                int? transportationId = stopDto.TransportationId > 0 ? stopDto.TransportationId : null;
                if (transportationId.HasValue)
                {
                    transportation = await _context.Transportations.FindAsync(transportationId.Value);
                    if (transportation == null)
                    {
                        errors.Add($"Transportation with Id {transportationId.Value} does not exist.");
                    }
                }
                
                if (errors.Count > 0)
                {
                    return new ApiResponse<StopResponseDTO>(400, errors);
                }
                var stop = new Stop
                {
                    StopOrder = stopDto.StopOrder,
                    DistanceFromPreviousStop = stopDto.DistanceFromPreviousStop,
                    CustomerId = stopDto.CustomerId,
                    AddressId = stopDto.AddressId,
                    TransportationId = transportationId,
                };
                
                _context.Stops.Add(stop);
                await _context.SaveChangesAsync(); 
                
                var deliveries = new List<Delivery>();
    
                foreach (var deliveryDto in stopDto.Deliveries)
                {
                    // Ensure the product exists
                    var delivery = await _context.Deliveries.FindAsync(deliveryDto.DeliveryId);
                    if (delivery == null)
                    {
                        return new ApiResponse<StopResponseDTO>(404,
                            $"DeliveryItem with ID {deliveryDto.DeliveryId} does not exist.");
                    }
                    
                    // delivery.Stop = stop;
                    delivery.StopId = stop.StopId;
                    _context.Deliveries.Update(delivery);
    
                    deliveries.Add(delivery);
                }
    
               
                stop.Deliveries = deliveries; 
                _context.Update(stop); 
                await _context.SaveChangesAsync();
                
                
                stop = await _context.Stops
                    .Include(s => s.Deliveries) 
                    .FirstOrDefaultAsync(s => s.StopId == stop.StopId);
    
                
                // Map the entity to a response DTO
                var stopResponse = MapStopToDTO(stop);
    
                return new ApiResponse<StopResponseDTO>(200, stopResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<StopResponseDTO>(500,
                    $"An unexpected error occurred while processing your request. Error: {ex.Message}");
            }
        }
    
    // public async Task<ApiResponse<StopResponseDTO>> CreateStopAsync(StopCreateDTO stopDto)
    //     {
    //         try
    //         {
    //             var address = await _context.Addresses.FindAsync(stopDto.AddressId);
    //             if (address == null)
    //             {
    //                 return new ApiResponse<StopResponseDTO>(404, $"Address with ID {stopDto.AddressId} does not exist.");
    //             }
    //             
    //             var customer = await _context.Customers.FindAsync(stopDto.CustomerId);
    //             if (customer == null)
    //             {
    //                 return new ApiResponse<StopResponseDTO>(404, $"Address with ID {stopDto.CustomerId} does not exist.");
    //             }
    //
    //             Transportation? transportation = null;
    //             int? transportationId = stopDto.TransportationId > 0 ? stopDto.TransportationId : null;
    //             if (transportationId.HasValue)
    //             {
    //                 transportation = await _context.Transportations.FindAsync(transportationId.Value);
    //                 if (transportation == null)
    //                 {
    //                     return new ApiResponse<StopResponseDTO>(404, "Transportation does not exist.");
    //                 }
    //             }
    //             
    //             var deliveries = new List<Delivery>();
    //
    //             foreach (var deliveryDto in stopDto.Deliveries)
    //             {
    //                 // Ensure the product exists
    //                 var delivery = await _context.Deliveries.FindAsync(deliveryDto.DeliveryId);
    //                 if (delivery == null)
    //                 {
    //                     return new ApiResponse<StopResponseDTO>(404,
    //                         $"DeliveryItem with ID {deliveryDto.DeliveryId} does not exist.");
    //                 }
    //
    //                 deliveries.Add(delivery);
    //             }
    //             
    //             var stop = new Stop
    //             {
    //                 StopOrder = stopDto.StopOrder,
    //                 DistanceFromPreviousStop = stopDto.DistanceFromPreviousStop,
    //                 CustomerId = stopDto.CustomerId,
    //                 AddressId = stopDto.AddressId,
    //                 Deliveries = deliveries,
    //                 TransportationId = transportationId,
    //             };
    //
    //             // Add and save the delivery
    //             _context.Stops.Add(stop);
    //             await _context.SaveChangesAsync();
    //             
    //             
    //             stop = await _context.Stops
    //                 .Include(s => s.Deliveries) 
    //                 .FirstOrDefaultAsync(s => s.StopId == stop.StopId);
    //
    //             // Map the entity to a response DTO
    //             var stopResponse = MapStopToDTO(stop);
    //
    //             return new ApiResponse<StopResponseDTO>(200, stopResponse);
    //         }
    //         catch (Exception ex)
    //         {
    //             return new ApiResponse<StopResponseDTO>(500,
    //                 $"An unexpected error occurred while processing your request. Error: {ex.Message}");
    //         }
    //     }
    
    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateStopStatusAsync(StopStatusUpdateDTO statusDto)
    {
        try
        {
            // Retrieve the order.
            var stop = await _context.Stops.FirstOrDefaultAsync(o => o.StopId == statusDto.StopId);
            if (stop == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Stop not found.");
            }
            var currentStatus = stop.Status;
            var newStatus = statusDto.StopStatus;
            // Validate the status transition.
            if (!AllowedStatusTransitions.TryGetValue(currentStatus, out var allowedStatuses))
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, "Current stop status is invalid.");
            }
            if (!allowedStatuses.Contains(newStatus))
            {
                return new ApiResponse<ConfirmationResponseDTO>(400, $"Cannot change stop status from {currentStatus} to {newStatus}.");
            }
            // Update the order status.
            stop.Status = newStatus;
            await _context.SaveChangesAsync();
            // Prepare a confirmation message.
            var confirmation = new ConfirmationResponseDTO
            {
                Message = $"Stop Status with Id {statusDto.StopId} updated successfully."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmation);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }
    
    public async Task<ApiResponse<List<StopResponseDTO>>> GetAllStopsAsync()
    {
        try
        {
            // Retrieve all stops with related entities.
            var stops = await _context.Stops
                .Include(s => s.Customer)
                .Include(s => s.Address)
                .Include(s => s.Deliveries) // ✅ Include Deliveries (collection)
                .ThenInclude(d => d.DeliveryItems) // ✅ Include DeliveryItems
                .Include(s => s.CurrentTransportation) // ✅ Include Transportation details
                .AsNoTracking()
                .ToListAsync();
            // Map each stop to its corresponding DTO.
            var stopList = stops.Select(s => MapStopToDTO(s)).ToList();

            return new ApiResponse<List<StopResponseDTO>>(200, stopList);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<StopResponseDTO>>(500, $"An unexpected error occurred while processing your request. Error: {ex.Message}");
        }
    }

    private StopResponseDTO MapStopToDTO(Stop stop)
    {
        var stopDeliveries = stop.Deliveries?.Select(s => new
            StopDeliveryResponse
            {
                Status = s.Status,
                TotalValue = s.TotalValue,
                TotalWeight = s.TotalWeight,
                TotalVolume = s.TotalVolume
            }).ToList() ?? new List<StopDeliveryResponse>();
        
        return new StopResponseDTO
        {
            StopId = stop.StopId,
            StopOrder = stop.StopOrder,
            DistanceFromPreviousStop = stop.DistanceFromPreviousStop,
            CustomerId = stop.CustomerId,
            AddressId = stop.AddressId,
            Deliveries = stopDeliveries,
            TransportationId = stop.TransportationId,
            Status = stop.Status
        };
    }
}


