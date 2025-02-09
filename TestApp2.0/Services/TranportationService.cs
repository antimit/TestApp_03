using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.TransportationDTOs;
using TestApp2._0.Models;

namespace TestApp2._0.Services;

public class TransportationService
{
    private readonly ApplicationDbContext _context;
    
    private static readonly Dictionary<TransportationStatus, List<TransportationStatus>> AllowedTransportationTransitions = new()
    {
        { TransportationStatus.Pending, new List<TransportationStatus> { TransportationStatus.InProgress, TransportationStatus.Canceled } },
        { TransportationStatus.InProgress, new List<TransportationStatus> { TransportationStatus.Completed, TransportationStatus.Canceled } },
        { TransportationStatus.Completed, new List<TransportationStatus>() }, // Terminal state
        { TransportationStatus.Canceled, new List<TransportationStatus>() }   // Terminal state
    };

    public TransportationService(ApplicationDbContext context)
    {
        _context = context;
    }
    
      public async Task<ApiResponse<TransportationResponseDTO>> CreateTransportationAsync(TransportationCreateDTO transportationDto)
        {
            try
            {
                var driver = await _context.Drivers.FindAsync(transportationDto.DriverId);
                if (driver == null)
                {
                    return new ApiResponse<TransportationResponseDTO>(404, $"Driver with ID {transportationDto.DriverId} does not exist.");
                }
                
                var truck = await _context.Vehicles.FindAsync(transportationDto.TruckId);
                if (truck == null)
                {
                    return new ApiResponse<TransportationResponseDTO>(404, $"Truck with ID {transportationDto.TruckId} does not exist.");
                }

                
                
                var Stops = new List<Stop>();
                
                foreach (var stopDto in transportationDto.Stops)
                {
                    // Ensure the product exists
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
                    CompletedStopsCount = 0,
                    IsFinalized = false
                };

                // Add and save the delivery
                _context.Transportations.Add(transportation);
                await _context.SaveChangesAsync();

                // Map the entity to a response DTO
                var transportationResponse = MapTransportationToDTO(transportation);

                return new ApiResponse<TransportationResponseDTO>(200, transportationResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<TransportationResponseDTO>(500,
                    $"An unexpected error occurred while processing your request. Error: {ex.Message}");
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
                Status = transportation.Status,
                CreatedAt = default,
                UpdatedAt = null,
                DepartureTime = null,
                ActualArrivalTime = null,
                LastStopId = null,
                CompletedStopsCount = 0,
                IsFinalized = false
            };
        }
    
}