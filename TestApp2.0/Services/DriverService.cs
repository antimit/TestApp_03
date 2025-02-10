using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.DriverDTOs;
using TestApp2._0.Models;

namespace TestApp2._0.Services;

public class DriverService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DriverService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    private static readonly Dictionary<DriverStatus, List<DriverStatus>> AllowedDriverStatusTransitions = new()
    {
        {
            DriverStatus.Active,
            new List<DriverStatus> { DriverStatus.OnLeave, DriverStatus.Suspended, DriverStatus.Retired }
        },
        { DriverStatus.OnLeave, new List<DriverStatus> { DriverStatus.Active, DriverStatus.Retired } },
        { DriverStatus.Suspended, new List<DriverStatus> { DriverStatus.Active, DriverStatus.Retired } },
        { DriverStatus.Retired, new List<DriverStatus>() }
    };

    public async Task<ApiResponse<DriverResponseDTO>> AddDriverAsync(DriverAddDTO driverDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            if (await _context.Drivers.AnyAsync(c => c.Email.ToLower() == driverDto.Email.ToLower()))
            {
                return new ApiResponse<DriverResponseDTO>(400, "Email already in use");
            }

            var driver = _mapper.Map<Driver>(driverDto);

            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
            
            await transaction.CommitAsync();

            var driverResponse = _mapper.Map<DriverResponseDTO>(driver);

            return new ApiResponse<DriverResponseDTO>(200, driverResponse);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();  
            return new ApiResponse<DriverResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");

        }
    }

    public async Task<ApiResponse<List<DriverResponseDTO>>> GetAllDriversAsync()
    {
        try
        {
            var drivers = await _context.Drivers
                .Include(o => o.Transportation)
                .AsNoTracking()
                .ToListAsync();

            var driversList = _mapper.Map<List<DriverResponseDTO>>(drivers);

            return new ApiResponse<List<DriverResponseDTO>>(200, driversList);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<DriverResponseDTO>>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }


    public async Task<ApiResponse<DriverResponseDTO>> GetDriverByIdAsync(int id)
    {
        try
        {
            var driver = await _context.Drivers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.DriverId == id);
            if (driver == null)
            {
                return new ApiResponse<DriverResponseDTO>(404, "Driver not found");
            }

            var driverResponse = _mapper.Map<DriverResponseDTO>(driver);

            return new ApiResponse<DriverResponseDTO>(200, driverResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<DriverResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }


    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateDriverStatusAsync(DriverUpdateStatusDTO statusDto)
    {
        try
        {
            var driver = await _context.Drivers.FirstOrDefaultAsync(o => o.DriverId == statusDto.DriverId);
            if (driver == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Driver not found");
            }

            var currentStatus = driver.Status;
            var newStatus = statusDto.DriverStatus;
            if (!AllowedDriverStatusTransitions.TryGetValue(currentStatus, out var allowedStatuses))
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, "Current driver status is invalid");
            }

            if (!allowedStatuses.Contains(newStatus))
            {
                return new ApiResponse<ConfirmationResponseDTO>(400,
                    $"Cannot change driver status from {currentStatus} to {newStatus}.");
            }

            driver.Status = newStatus;

            await _context.SaveChangesAsync();

            var confirmation = new ConfirmationResponseDTO()
            {
                Message = $"Driver's Status with Id {statusDto.DriverId} updated successfully."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmation);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }
    
}