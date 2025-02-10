using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.DeliveryItemsDTOs;
using TestApp2._0.Models;

namespace TestApp2._0.Services;

public class DeliveryItemService
{
    private readonly ApplicationDbContext _context;

    private readonly IMapper _mapper;

    public DeliveryItemService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<DeliveryItemResponseDTO>> CreateDeliveryItemAsync(
        DeliveryItemCreateDTO deliveryItemDto)
    {
        try
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == deliveryItemDto.productId);

            if (product == null)
            {
                return new ApiResponse<DeliveryItemResponseDTO>(400, "Invalid Product ID.");
            }

            decimal totalCost = deliveryItemDto.OrderedCount * product.SalesUnitPrice;


            var deliveryItem = _mapper.Map<DeliveryItem>(deliveryItemDto);

            _context.DeliveryItems.Add(deliveryItem);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<DeliveryItemResponseDTO>(deliveryItem);

            return new ApiResponse<DeliveryItemResponseDTO>(200, response);
        }
        catch (Exception ex)
        {
            return new ApiResponse<DeliveryItemResponseDTO>(500, $"Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<DeliveryItemResponseDTO>>> GetAllDeliveryItemsAsync()
    {
        try
        {
            var deliveryItems = await _context.DeliveryItems
                .AsNoTracking()
                .ToListAsync();

            var responseList = _mapper.Map <List<DeliveryItemResponseDTO>>(deliveryItems);

            return new ApiResponse<List<DeliveryItemResponseDTO>>(200, responseList);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<DeliveryItemResponseDTO>>(500, $"Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<DeliveryItemResponseDTO>>> GetFilteredDeliveryItemsAsync(
        decimal? maxsalesUnitPrice, int? minItemWeight)
    {
        try
        {
            var query = _context.DeliveryItems.AsQueryable();

            if (maxsalesUnitPrice.HasValue)
            {
                query = query.Where(di => di.SalesUnitPrice <= maxsalesUnitPrice.Value);
            }

            if (minItemWeight.HasValue)
            {
                query = query.Where(di => di.ItemWeight > minItemWeight.Value);
            }

            var deliveryItems = await query
                .AsNoTracking()
                .ToListAsync();

            var responseList = _mapper.Map <List<DeliveryItemResponseDTO>>(deliveryItems);

            return new ApiResponse<List<DeliveryItemResponseDTO>>(200, responseList);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<DeliveryItemResponseDTO>>(500, $"Error: {ex.Message}");
        }
    }


    public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteDeliveryItemAsync(int deliveryItemId)
    {
        try
        {
            var deliveryItem = await _context.DeliveryItems.FindAsync(deliveryItemId);
            if (deliveryItem == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "DeliveryItem not found.");
            }

            _context.DeliveryItems.Remove(deliveryItem);
            await _context.SaveChangesAsync();
            var confirmation = new ConfirmationResponseDTO
            {
                Message = $"DeliveryItem with Id {deliveryItem.DeliveryItemId} deleted successfully."
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