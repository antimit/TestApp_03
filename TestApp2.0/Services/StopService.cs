using AutoMapper;
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
    private readonly IMapper _mapper;
    
    public StopService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    private static readonly Dictionary<StopStatus, List<StopStatus>> AllowedStatusTransitions = new()
    {
        { StopStatus.Pending, new List<StopStatus> { StopStatus.Arrived, StopStatus.Skipped } },
        { StopStatus.Arrived, new List<StopStatus> { StopStatus.Completed, StopStatus.Skipped } },
        { StopStatus.Completed, new List<StopStatus>() },
        { StopStatus.Skipped, new List<StopStatus>() }
    };

    

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

            

            if (errors.Count > 0)
            {
                return new ApiResponse<StopResponseDTO>(400, errors);
            }

            var stop = _mapper.Map<Stop>(stopDto);
            stop.TransportationId = null;

            _context.Stops.Add(stop);
            await _context.SaveChangesAsync();

            var deliveries = new List<Delivery>();

            foreach (var deliveryDto in stopDto.Deliveries)
            {
                var delivery = await _context.Deliveries.FindAsync(deliveryDto.DeliveryId);
                if (delivery == null)
                {
                    return new ApiResponse<StopResponseDTO>(404,
                        $"DeliveryItem with ID {deliveryDto.DeliveryId} does not exist.");
                }

                delivery.StopId = stop.StopId;
                _context.Deliveries.Update(delivery);

                deliveries.Add(delivery);
            }


            stop.Deliveries = deliveries;
            await _context.SaveChangesAsync();


            stop = await _context.Stops
                .Include(s => s.Deliveries)
                .FirstOrDefaultAsync(s => s.StopId == stop.StopId);


            var stopResponse = _mapper.Map<StopResponseDTO>(stop);

            return new ApiResponse<StopResponseDTO>(200, stopResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<StopResponseDTO>(500,
                $"An unexpected error occurred while processing your request. Error: {ex.Message}");
        }
    }
    
    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateStopStatusAsync(StopStatusUpdateDTO statusDto)
    {
        try
        {
            
            var stop = await _context.Stops.FirstOrDefaultAsync(o => o.StopId == statusDto.StopId);
            if (stop == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Stop not found.");
            }

            var currentStatus = stop.Status;
            var newStatus = statusDto.StopStatus;
            
            if (!AllowedStatusTransitions.TryGetValue(currentStatus, out var allowedStatuses))
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, "Current stop status is invalid.");
            }

            if (!allowedStatuses.Contains(newStatus))
            {
                return new ApiResponse<ConfirmationResponseDTO>(400,
                    $"Cannot change stop status from {currentStatus} to {newStatus}.");
            }

            
            stop.Status = newStatus;
            await _context.SaveChangesAsync();
            
            var confirmation = new ConfirmationResponseDTO
            {
                Message = $"Stop Status with Id {statusDto.StopId} updated successfully."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmation);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<StopResponseDTO>>> GetAllStopsAsync()
    {
        try
        {
            var stops = await _context.Stops
                .Include(s => s.Customer)
                .Include(s => s.Address)
                .Include(s => s.Deliveries) 
                .ThenInclude(d => d.DeliveryItems) 
                .Include(s => s.CurrentTransportation) 
                .AsNoTracking()
                .ToListAsync();
            var stopList = _mapper.Map<List<StopResponseDTO>>(stops);

            return new ApiResponse<List<StopResponseDTO>>(200, stopList);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<StopResponseDTO>>(500,
                $"An unexpected error occurred while processing your request. Error: {ex.Message}");
        }
    }
   
}