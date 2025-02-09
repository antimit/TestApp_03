using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.DeliveryDTOs;
using TestApp2._0.DTOs.VehicleDTOs;
using TestApp2._0.Models;

namespace TestApp2._0.Services;

public class VehicleService
{
    private readonly ApplicationDbContext _context;
    
    private static readonly Dictionary<VehicleStatus, List<VehicleStatus>> AllowedStatusTransitions = new()
    {
        { VehicleStatus.Active, new List<VehicleStatus> { VehicleStatus.UnderMaintenance, VehicleStatus.Reserved, VehicleStatus.Retired } },
        { VehicleStatus.UnderMaintenance, new List<VehicleStatus> { VehicleStatus.Active, VehicleStatus.Retired } },
        { VehicleStatus.Reserved, new List<VehicleStatus> { VehicleStatus.Active } },
        { VehicleStatus.Retired, new List<VehicleStatus>() } // Terminal state
    };

    public VehicleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<VehicleResponseDTO>> CreateVehicleAsync(VehicleAddDTO vehicleDto)
    {
        try
        {
            var licensePlate = await _context.Vehicles.AnyAsync(c => c.LicensePlate == vehicleDto.LicensePlate);

            if (licensePlate)
            {
                return new ApiResponse<VehicleResponseDTO>(400, "Vehicle with this LicensePlate already in use");
            }

            var vehicle = new Vehicle()
            {
                LicensePlate = vehicleDto.LicensePlate,
                Make = vehicleDto.Make,
                Model = vehicleDto.Model,
                VIN = vehicleDto.VIN,
                Status = VehicleStatus.Active,
                IsAvailable = true
            };

            _context.Vehicles.Add(vehicle);

            await _context.SaveChangesAsync();
            
            
            var savedVehicle = await _context.Vehicles
                .Include(v => v.Transportations) 
                .FirstOrDefaultAsync(v => v.VehicleId == vehicle.VehicleId);
            
            if (savedVehicle == null)
            {
                return new ApiResponse<VehicleResponseDTO>(500, "Failed to retrieve saved vehicle.");
            }
            
            var vehicleResponse = MapVehicleToDTO(savedVehicle);

            return new ApiResponse<VehicleResponseDTO>(200, vehicleResponse);
        }

        catch(Exception ex)
        {
            return new ApiResponse<VehicleResponseDTO>(500,
                $"An unexpected error occurred while adding  your vehicle, Error: {ex.Message}");
        }
    }


    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateVehicleStatusAsync(VehicleStatusUpdateDTO statusDto)
    {
        try
        {
            
            
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(o => o.VehicleId == statusDto.VehicleId);
            if (vehicle == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Vehicle not found");
            }

            var currentStatus = vehicle.Status;
            var newStatus = statusDto.VehicleStatus;
            if (!AllowedStatusTransitions.TryGetValue(currentStatus, out var allowedStatuses))
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, "Current vehicle status is invalid");
            }

            if (!allowedStatuses.Contains(newStatus))
            {
                return new ApiResponse<ConfirmationResponseDTO>(400,
                    $"Cannot change vehicle status from {currentStatus} to {newStatus}.");
            }

            vehicle.Status = newStatus;

            await _context.SaveChangesAsync();

            var confirmation = new ConfirmationResponseDTO()
            {
                Message = $"Vehicle Status with Id {statusDto.VehicleId} updated successfully."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmation);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
        
        
    }


    public async Task<ApiResponse<List<VehicleResponseDTO>>> GetAllVehiclesAsync()
    {
        try
        {
            var vehicles = await _context.Vehicles
                .Include(o => o.Transportations)
                .AsNoTracking()
                .ToListAsync();

            var vehiclesList = vehicles.Select(o =>MapVehicleToDTO(o)).ToList();
            return new ApiResponse<List<VehicleResponseDTO>>(200, vehiclesList);

        }
        catch (Exception ex)
        {
            return new ApiResponse<List<VehicleResponseDTO>>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }
    
    
   
    private VehicleResponseDTO MapVehicleToDTO(Vehicle vehicle)
    {
            
            
            return new VehicleResponseDTO
            {
                VehicleId = vehicle.VehicleId,
                LicensePlate = vehicle.LicensePlate,
                Make = vehicle.Make,
                Model = vehicle.Model,
                VIN = vehicle.VIN,
                IsAvailable = true,
                Status = VehicleStatus.Active
            };
    }
    
    
    
}