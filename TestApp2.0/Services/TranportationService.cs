using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.TransportationDTOs;
using TestApp2._0.Models;

namespace TestApp2._0.Services;

public class TransportationService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public TransportationService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    private static readonly Dictionary<TransportationStatus, List<TransportationStatus>>
        AllowedTransportationTransitions = new()
        {
            {
                TransportationStatus.Pending,
                new List<TransportationStatus> { TransportationStatus.InProgress, TransportationStatus.Canceled }
            },
            {
                TransportationStatus.InProgress,
                new List<TransportationStatus> { TransportationStatus.Completed, TransportationStatus.Canceled }
            },
            { TransportationStatus.Completed, new List<TransportationStatus>() },
            { TransportationStatus.Canceled, new List<TransportationStatus>() }
        };

   

    public async Task<ApiResponse<TransportationResponseDTO>> CreateTransportationAsync(
        TransportationCreateDTO transportationDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var driver = await _context.Drivers.FindAsync(transportationDto.DriverId);
            if (driver == null)
            {
                return new ApiResponse<TransportationResponseDTO>(404,
                    $"Driver with ID {transportationDto.DriverId} does not exist.");
            }

            var truck = await _context.Vehicles.FindAsync(transportationDto.TruckId);
            if (truck == null)
            {
                return new ApiResponse<TransportationResponseDTO>(404,
                    $"Truck with ID {transportationDto.TruckId} does not exist.");
            }




            var transportation = _mapper.Map<Transportation>(transportationDto);

            _context.Transportations.Add(transportation);
            await _context.SaveChangesAsync();
            
            var Stops = new List<Stop>();

            foreach (var stopDto in transportationDto.Stops)
            {
                var stop = await _context.Stops.FindAsync(stopDto.StopId);
                if (stop == null)
                {
                    await transaction.RollbackAsync();  
                    return new ApiResponse<TransportationResponseDTO>(404,
                        $"Stop with ID {stopDto.StopId} does not exist.");
                }

                stop.TransportationId = transportation.TransportationId;
                
                _context.Stops.Update(stop);

                Stops.Add(stop);
            }
            
            transportation.Stops = Stops;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var transportationResponse = _mapper.Map<TransportationResponseDTO>(transportation);

            return new ApiResponse<TransportationResponseDTO>(200, transportationResponse);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();  
            return new ApiResponse<TransportationResponseDTO>(500,
                $"An unexpected error occurred while processing your request. Error: {ex.Message}");
        }
    }


    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateTransportationStatusAsync(
        TransportationStatusUpdateDTO statusDto)
    {
        try
        {
            var transportation =
                await _context.Transportations.FirstOrDefaultAsync(
                    t => t.TransportationId == statusDto.TransportationId);

            if (transportation == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Transportation record not found");
            }
            
            var currentStatus = transportation.TransportationStatus;
            var newStatus = statusDto.TransportationStatus;

            if (!AllowedTransportationTransitions.TryGetValue(currentStatus, out var allowedStatuses))
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, "Current transportation status is invalid");
            }

            if (!allowedStatuses.Contains(newStatus))
            {
                return new ApiResponse<ConfirmationResponseDTO>(400,
                    $"Cannot change transportation status from {currentStatus} to {newStatus}.");
            }

            transportation.TransportationStatus = newStatus;
            transportation.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var confirmation = new ConfirmationResponseDTO()
            {
                Message = $"Transportation status with ID {statusDto.TransportationId} updated successfully."
            };

            return new ApiResponse<ConfirmationResponseDTO>(200, confirmation);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }


    public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteTransportationAsync(int transportationId)
    {
        try
        {
            var transportation = await _context.Transportations.FindAsync(transportationId);
            if (transportation == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Transportation not found.");
            }

            _context.Transportations.Remove(transportation);
            await _context.SaveChangesAsync();
            var confirmation = new ConfirmationResponseDTO
            {
                Message = $"Transportation with Id {transportation.TransportationId} deleted successfully."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmation);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }
    
    
    public async Task<ApiResponse<List<TransportationResponseDTO>>> GetAllTransportationsAsync()
    {
        try
        {
            var transportations = await _context.Transportations
                .Include(t => t.Driver)
                .Include(t => t.Truck)
                .Include(t => t.Stops)
                .ThenInclude(s => s.Address)
                .AsNoTracking()
                .ToListAsync();

            var transportationList = _mapper.Map<List<TransportationResponseDTO>>(transportations);

            return new ApiResponse<List<TransportationResponseDTO>>(200, transportationList);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<TransportationResponseDTO>>(500,
                $"An unexpected error occurred while processing your request. Error: {ex.Message}");
        }
    }
    
}