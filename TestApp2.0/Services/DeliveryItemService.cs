using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.DeliveryItemsDTOs;
using TestApp2._0.Models;

namespace TestApp2._0.Services;

public class DeliveryItemService
{
    private readonly ApplicationDbContext _context;

    public DeliveryItemService(ApplicationDbContext context)
    {
        _context = context;
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

            int? deliveryId = deliveryItemDto.CurrentDeliveryId == 0 ? null : deliveryItemDto.CurrentDeliveryId;

            var deliveryItem = new DeliveryItem
            {
                ProductId = product.ProductId,
                Name = product.Name, SalesUnitPrice = product.SalesUnitPrice,
                OrderedCount = deliveryItemDto.OrderedCount,
                DeliveredCount = deliveryItemDto.DeliveredCount ?? 0, CurrentDeliveryId = deliveryId,
                TotalCost = totalCost, ItemWeight = product.Weight, ItemVolume = product.Volume
            };

            _context.DeliveryItems.Add(deliveryItem);
            await _context.SaveChangesAsync();

            var response = new DeliveryItemResponseDTO
            {
                DeliveryItemId = deliveryItem.DeliveryItemId,
                Name = deliveryItem.Name,
                SalesUnitPrice = deliveryItem.SalesUnitPrice,
                OrderedCount = deliveryItem.OrderedCount,
                DeliveredCount = deliveryItem.DeliveredCount,
                CurrentDeliveryId = deliveryItem.CurrentDeliveryId,
                TotalCost = deliveryItem.TotalCost,
                ItemWeight = deliveryItem.ItemWeight,
                ItemVolume = deliveryItem.ItemVolume
            };

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

            var responseList = deliveryItems.Select(di => new DeliveryItemResponseDTO
            {
                DeliveryItemId = di.DeliveryItemId,
                Name = di.Name,
                SalesUnitPrice = di.SalesUnitPrice,
                OrderedCount = di.OrderedCount,
                DeliveredCount = di.DeliveredCount,
                CurrentDeliveryId = di.CurrentDeliveryId,
                TotalCost = di.TotalCost,
                ItemWeight = di.ItemWeight,
                ItemVolume = di.ItemVolume
            }).ToList();

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

            var responseList = deliveryItems.Select(di => new DeliveryItemResponseDTO
            {
                DeliveryItemId = di.DeliveryItemId,
                Name = di.Name,
                SalesUnitPrice = di.SalesUnitPrice,
                OrderedCount = di.OrderedCount,
                DeliveredCount = di.DeliveredCount,
                CurrentDeliveryId = di.CurrentDeliveryId,
                TotalCost = di.TotalCost,
                ItemWeight = di.ItemWeight,
                ItemVolume = di.ItemVolume
            }).ToList();

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