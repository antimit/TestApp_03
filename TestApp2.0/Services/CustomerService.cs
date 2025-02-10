using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.CustomerDTOs;
using TestApp2._0.Models;
using AutoMapper;

namespace TestApp2._0.Services;

public class CustomerService
{
    private readonly ApplicationDbContext _context;
    
    private readonly IMapper _mapper;
    

    public CustomerService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<ApiResponse<List<CustomerResponseDTO>>> GetAllCustomersAsync()
    {
        try
        {
            var customers = await _context.Customers
                .Include(o => o.Stop)
                .AsNoTracking()
                .ToListAsync();

            var customerList = _mapper.Map <List<CustomerResponseDTO>>(customers);

            return new ApiResponse<List<CustomerResponseDTO>>(200, customerList);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CustomerResponseDTO>>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CustomerResponseDTO>> AddCustomerAsync(CustomerAddDTO customerDto)
    {
        try
        {
            if (await _context.Customers.AnyAsync(c => c.Email.ToLower() == customerDto.Email.ToLower()))
            {
                return new ApiResponse<CustomerResponseDTO>(400, "Email already in use");
            }

            var customer = _mapper.Map<Customer>(customerDto);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var customerResponse = _mapper.Map<CustomerResponseDTO>(customer);

            return new ApiResponse<CustomerResponseDTO>(200, customerResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<CustomerResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }


    public async Task<ApiResponse<CustomerResponseDTO>> GetCustomerByIdAsync(int id)
    {
        try
        {
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerId == id);
            if (customer == null)
            {
                return new ApiResponse<CustomerResponseDTO>(404, "Customer not found");
            }

            var customerResponse = _mapper.Map<CustomerResponseDTO>(customer);

            return new ApiResponse<CustomerResponseDTO>(200, customerResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<CustomerResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }


    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateCustomerAsync(CustomerUpdateDTO customerDto)
    {
        try
        {
            var customer = await _context.Customers.FindAsync(customerDto.CustomerId);
            if (customer == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found.");
            }

            if (customer.Email != customerDto.Email &&
                await _context.Customers.AnyAsync(c => c.Email == customerDto.Email))
            {
                return new ApiResponse<ConfirmationResponseDTO>(400, "Email is already in use.");
            }

            customer.FirstName = customerDto.FirstName;
            customer.LastName = customerDto.LastName;
            customer.Email = customerDto.Email;
            customer.PhoneNumber = customerDto.PhoneNumber;

            await _context.SaveChangesAsync();
            var confirmationMessage = new ConfirmationResponseDTO
            {
                Message = $"Customer with Id {customerDto.CustomerId} updated successfully."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteCustomerAsync(int id)
    {
        try
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id);
            if (customer == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found.");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            var confirmationMessage = new ConfirmationResponseDTO
            {
                Message = $"Customer with Id {id} deleted successfully."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }
}