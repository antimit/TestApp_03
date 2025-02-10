using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.TransportationDTOs;
using TestApp2._0.Models;

namespace TestApp2._0.Services;

public class TransportationService
{
    private readonly ApplicationDbContext _context;

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

    public TransportationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<TransportationResponseDTO>> CreateTransportationAsync(
        TransportationCreateDTO transportationDto)
    {
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


            var Stops = new List<Stop>();

            foreach (var stopDto in transportationDto.Stops)
            {
                var stop = await _context.Stops.FindAsync(stopDto.StopId);
                if (stop == null)
                {
                    return new ApiResponse<TransportationResponseDTO>(404,
                        $"Stop with ID {stopDto.StopId} does not exist.");
                }

                Stops.Add(stop);
            }

            var transportation = new Transportation
            {
                DriverId = transportationDto.DriverId,
                TruckId = transportationDto.TruckId,
                Stops = Stops,
                CreatedAt = default,
                UpdatedAt = null,
                DepartureTime = null,
                ActualArrivalTime = null,
                LastStopId = null,
                CompletedStopsCount = 0
            };

            _context.Transportations.Add(transportation);
            await _context.SaveChangesAsync();

            var transportationResponse = MapTransportationToDTO(transportation);

            return new ApiResponse<TransportationResponseDTO>(200, transportationResponse);
        }
        catch (Exception ex)
        {
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


    private TransportationResponseDTO MapTransportationToDTO(Transportation transportation)
    {
        return new TransportationResponseDTO
        {
            TransportationId = transportation.TransportationId,
            DriverId = transportation.DriverId,
            TruckId = transportation.TruckId,
            Stops = transportation.Stops,
            Status = transportation.TransportationStatus,
            CreatedAt = default,
            UpdatedAt = null,
            DepartureTime = null,
            ActualArrivalTime = null,
            LastStopId = null,
            CompletedStopsCount = 0
        };
    }
}