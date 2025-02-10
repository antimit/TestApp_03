using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.AddressDTOs;
using TestApp2._0.Models;
using AutoMapper;


namespace TestApp2._0.Services;

public class AddressService
{
    private readonly ApplicationDbContext _context;
    
    private readonly IMapper _mapper;

    public AddressService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<AddressResponseDTO>> AddAddressAsync(AddressAddDTO addressDto)
    {
        try
        {
            var address = _mapper.Map<Address>(addressDto);

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            var addressResponse = _mapper.Map<AddressResponseDTO>(address);

            return new ApiResponse<AddressResponseDTO>(200, addressResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<AddressResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<AddressResponseDTO>> GetAddressByIdAsync(int id)
    {
        try
        {
            var address = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(add => add.AddressId == id);
            if (address == null)
            {
                return new ApiResponse<AddressResponseDTO>(404, "Address not found.");
            }

            var addressResponse = _mapper.Map<AddressResponseDTO>(address);
            return new ApiResponse<AddressResponseDTO>(200, addressResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<AddressResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }


    public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteAddressAsync(int addressId)
    {
        try
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(add => add.AddressId == addressId);
            if (address == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found.");
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            var confirmationMessage = new ConfirmationResponseDTO
            {
                Message = $"Address with Id {addressId} deleted successfully."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500,
                "An unexpected error occurred while processing your request.");
        }
    }
}