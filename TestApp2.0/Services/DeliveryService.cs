using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.DeliveryDTOs;
using TestApp2._0.Models;

namespace TestApp2._0.Services
{
    public class DeliveryService
    {
        private readonly ApplicationDbContext _context;

        public DeliveryService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        private static readonly Dictionary<DeliveryStatus, List<DeliveryStatus>> AllowedStatusTransitions = new()
        {
            { DeliveryStatus.Pending, new List<DeliveryStatus> { DeliveryStatus.InTransit, DeliveryStatus.Canceled } },
            { DeliveryStatus.InTransit, new List<DeliveryStatus> { DeliveryStatus.Delivered, DeliveryStatus.Failed } },
            { DeliveryStatus.Delivered, new List<DeliveryStatus>() }, // Terminal state
            { DeliveryStatus.Failed, new List<DeliveryStatus>() },    // Terminal state
            { DeliveryStatus.Canceled, new List<DeliveryStatus>() }   // Terminal state
        };


        public async Task<ApiResponse<DeliveryResponseDTO>> CreateDeliveryAsync(DeliveryCreateDTO deliveryDto)
        {
            try
            {


                Stop? stop = null;
                int? stopId = deliveryDto.StopId > 0 ? deliveryDto.StopId : null;
                if (stopId.HasValue)
                {
                    stop = await _context.Stops.FindAsync(stopId.Value);
                    if (stop == null)
                    {
                        return new ApiResponse<DeliveryResponseDTO>(404, "Stop does not exist.");
                    }
                }
                
                var deliveryItems = new List<DeliveryItem>();

                foreach (var itemDto in deliveryDto.DeliveryItems)
                {
                    // Ensure the product exists
                    var deliveryItem = await _context.DeliveryItems.FindAsync(itemDto.DeliveryItemId);
                    if (deliveryItem == null)
                    {
                        return new ApiResponse<DeliveryResponseDTO>(404,
                            $"DeliveryItem with ID {itemDto.DeliveryItemId} does not exist.");
                    }

                    deliveryItems.Add(deliveryItem);
                }

                // Create the Delivery entity
                var delivery = new Delivery
                {
                    StopId = stopId,
                    Stop = stop,
                    DeliveryItems = deliveryItems,
                    Status = deliveryDto.Status
                };

                // Add and save the delivery
                _context.Deliveries.Add(delivery);
                await _context.SaveChangesAsync();

                // Map the entity to a response DTO
                var deliveryResponse = MapDeliveryToDTO(delivery);

                return new ApiResponse<DeliveryResponseDTO>(200, deliveryResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeliveryResponseDTO>(500,
                    $"An unexpected error occurred while processing your request. Error: {ex.Message}");
            }
        }
        
        public async Task<ApiResponse<DeliveryResponseDTO>> GetDeliveryByIdAsync(int deliveryId)
        {
            try
            {
                // Retrieve the delivery with related entities
                var delivery = await _context.Deliveries
                    .Include(d => d.Stop) // ✅ Include Stop details
                    .ThenInclude(s => s.Customer) // ✅ Include Customer inside Stop, if applicable
                    .Include(d => d.DeliveryItems) // ✅ Include Delivery Items
                    .ThenInclude(di => di.Product) // ✅ Include Product inside Delivery Items
                    .FirstOrDefaultAsync(d => d.DeliveryId == deliveryId); // ✅ Use the correct property name

                if (delivery == null)
                {
                    return new ApiResponse<DeliveryResponseDTO>(404, "Order not found.");
                }

                // ✅ Pass 'delivery' to the mapping function
                var deliveryResponse = MapDeliveryToDTO(delivery);

                return new ApiResponse<DeliveryResponseDTO>(200, deliveryResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeliveryResponseDTO>(500, 
                    $"An unexpected error occurred while processing your request. Error: {ex.Message}");
            }
        }
        
        
        public async Task<ApiResponse<List<DeliveryResponseDTO>>> GetAllDeliveriesAsync()
        {
            try
            {
                // Retrieve all orders with related entities.
                // Retrieve the delivery with related entities
                var deliveries = await _context.Deliveries
                    .Include(d => d.Stop) // ✅ Include Stop details
                    .ThenInclude(s => s.Customer) // ✅ Include Customer inside Stop, if applicable
                    .Include(d => d.DeliveryItems) // ✅ Include Delivery Items
                    .ThenInclude(di => di.Product) // ✅ Include Product inside Delivery Items
                    .ToListAsync();
                // Map each order to its corresponding DTO.
                var deliveryList = deliveries.Select(o => MapDeliveryToDTO(o)).ToList();
                return new ApiResponse<List<DeliveryResponseDTO>>(200, deliveryList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<DeliveryResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        
        
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateDeliveryStatusAsync(DeliveryStatusUpdateDTO statusDto)
        {
            try
            {
                // Retrieve the order.
                var delivery = await _context.Deliveries.FirstOrDefaultAsync(o => o.DeliveryId == statusDto.DeliveryId);
                if (delivery == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Delivery not found.");
                }
                
                var currentStatus = delivery.Status;
                var newStatus = statusDto.DeliveryStatus;
                // Validate the status transition.
                if (!AllowedStatusTransitions.TryGetValue(currentStatus, out var allowedStatuses))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(500, "Current delivery status is invalid.");
                }
                if (!allowedStatuses.Contains(newStatus))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, $"Cannot change delivery status from {currentStatus} to {newStatus}.");
                }
                // Update the order status.
                delivery.Status = newStatus;
                await _context.SaveChangesAsync();
                // Prepare a confirmation message.
                var confirmation = new ConfirmationResponseDTO
                {
                    Message = $"Delivery Status with Id {statusDto.DeliveryId} updated successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmation);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        private DeliveryResponseDTO MapDeliveryToDTO(Delivery delivery)
        {
            var DeliveryItems = delivery.DeliveryItems?.Select(s => new
                DeliveryDeliveryItemResponseDTO
                {
                    ProductId = s.ProductId,
                    SalesUnitPrice = s.SalesUnitPrice,
                    TotalCost = s.TotalCost,
                    ItemWeight = s.ItemWeight,
                    ItemVolume = s.ItemVolume
                }).ToList() ?? new List<DeliveryDeliveryItemResponseDTO>();
            return new DeliveryResponseDTO
            {
                DeliveryId = delivery.DeliveryId,
                StopId = delivery.StopId,
                Status = delivery.Status,
                DeliveryItems = DeliveryItems

            };
        }
    }
}
