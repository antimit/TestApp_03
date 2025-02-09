using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.CustomerDTOs;
using TestApp2._0.Models;

namespace TestApp2._0.Services;

public class CustomerService
{
    private readonly ApplicationDbContext _context;

    public CustomerService(ApplicationDbContext context)
    {
        _context = context;
    }

    
    public async Task<ApiResponse<List<CustomerResponseDTO>>> GetAllCustomersAsync()
    {
        try
        {
            var customers = await _context.Customers
                .Include(o =>o.Stop)
                .AsNoTracking()
                .ToListAsync();

            var customerList = customers.Select(o => MapCustomerToDTO(o,o?.Stop)).ToList();

            return new ApiResponse<List<CustomerResponseDTO>>(200, customerList);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CustomerResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
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

            var customer = new Customer
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                PhoneNumber = customerDto.PhoneNumber,
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            
            var customerResponse = new CustomerResponseDTO
            {
                Id = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                
            };

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

            var customerResponse = new CustomerResponseDTO()
            {
                Id = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
            };

            return new ApiResponse<CustomerResponseDTO>(200, customerResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<CustomerResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
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
            // Check if email is being updated to an existing one
            if (customer.Email != customerDto.Email && await _context.Customers.AnyAsync(c => c.Email == customerDto.Email))
            {
                return new ApiResponse<ConfirmationResponseDTO>(400, "Email is already in use.");
            }
            // Update customer properties manually
            customer.FirstName = customerDto.FirstName;
            customer.LastName = customerDto.LastName;
            customer.Email = customerDto.Email;
            customer.PhoneNumber = customerDto.PhoneNumber;
            
            await _context.SaveChangesAsync();
            // Prepare confirmation message
            var confirmationMessage = new ConfirmationResponseDTO
            {
                Message = $"Customer with Id {customerDto.CustomerId} updated successfully."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
        }
        catch (Exception ex)
        {
            // Log the exception
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
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
            //Soft Delete
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            // Prepare confirmation message
            var confirmationMessage = new ConfirmationResponseDTO
            {
                Message = $"Customer with Id {id} deleted successfully."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
        }
        catch (Exception ex)
        {
            // Log the exception
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }
    
    
    private CustomerResponseDTO MapCustomerToDTO(Customer customer, Stop stop)
    {
        
        
        return new CustomerResponseDTO
        {
            Id = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            stopOrder = stop?.StopOrder ?? "No StopOrder yet"
            
        };
    }
    
    
    
}