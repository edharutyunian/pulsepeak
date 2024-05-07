using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.Enums;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.BLL.Operations
{
    public class OrderOperations : IOrderOperations
    {
        private readonly ILogger log;
        private readonly IRepositoryHandler repositoryHandler;
        private readonly IMapper mapper;
        private string errorMessage;

        public OrderOperations(ILogger logger, IRepositoryHandler repositoryHandler, IMapper mapper)
        {
            this.log = logger;
            this.mapper = mapper;
            this.repositoryHandler = repositoryHandler;
            this.errorMessage = string.Empty;
        }

        // TODO: Implement
        public Task<OrderModel> CreateOrder(ShoppingCartModel shoppingCart)
        {
            throw new NotImplementedException();
        }

        // TODO: Implement
        public Task<OrderModel> Checkout(long orderId, ShoppingCartModel shoppingCartModel)
        {
            // just pay?
            // add something to the Payment operations/repo?
            throw new NotImplementedException();
        }

        public async Task<OrderModel> GetOrder(long orderId)
        {
            try {
                var order = await this.repositoryHandler.OrderRepository.GetSingleAsync(x => x.Id == orderId)
                    ?? throw new EntityNotFoundException($"Order with id '{orderId}' not found");

                return this.mapper.Map<OrderModel>(order);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<IEnumerable<OrderModel>> GetCustomerOrders(long customerId)
        {
            try {
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == customerId)
                    ?? throw new EntityNotFoundException($"Customer with id '{customerId}' not found");

                if (customer.Orders == null) {
                    return Enumerable.Empty<OrderModel>();
                }

                var customersOrder = new List<OrderModel>();
                foreach (var orderEntity in customer.Orders) {
                    var order = this.mapper.Map<OrderModel>(orderEntity);
                    customersOrder.Add(order);
                }

                return customersOrder;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> MarkAsShipped(long orderId)
        {
            try {
                var order = await this.repositoryHandler.OrderRepository.GetSingleAsync(x => x.Id == orderId)
                    ?? throw new EntityNotFoundException($"Order with id '{orderId}' not found");

                order.IsShipped = true;
                order.ShippingDate = order.OrderDate.AddDays(2);
                bool isOrderUpdated = this.repositoryHandler.OrderRepository.Update(order);
                if (!isOrderUpdated) {
                    throw new DbContextException($"The {nameof(order)} has not been updated.");
                }

                return isOrderUpdated;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> MarkAsDelivered(long orderId)
        {
            try {
                var order = await this.repositoryHandler.OrderRepository.GetSingleAsync(x => x.Id == orderId)
                    ?? throw new EntityNotFoundException($"Order with id '{orderId}' not found");

                order.IsDelivered = true;
                order.DeliveryDate = order.OrderDate.AddDays(2);
                bool isOrderUpdated = this.repositoryHandler.OrderRepository.Update(order);
                if (!isOrderUpdated) {
                    throw new DbContextException($"The {nameof(order)} has not been updated.");
                }

                return isOrderUpdated;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> MarkAsCanceled(long orderId)
        {
            try {
                var order = await this.repositoryHandler.OrderRepository.GetSingleAsync(x => x.Id == orderId)
                   ?? throw new EntityNotFoundException($"Order with id '{orderId}' not found");

                order.IsCanceled = true;
                bool isOrderUpdated = this.repositoryHandler.OrderRepository.Update(order);
                if (!isOrderUpdated) {
                    throw new DbContextException($"The {nameof(order)} has not been updated.");
                }

                return isOrderUpdated;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> UpdateOrderStatus(long orderId, OrderPlacementStatus orderPlacementStatus)
        {
            try {
                if (!Enum.IsDefined(typeof(ProductAvailabilityStatus), orderPlacementStatus)) {
                    throw new ArgumentOutOfRangeException();
                }

                var order = await this.repositoryHandler.OrderRepository.GetSingleAsync(x => x.Id == orderId)
                    ?? throw new EntityNotFoundException($"Order with id '{orderId}' not found");

                order.OrderStatus = orderPlacementStatus;
                bool isOrderUpdated = this.repositoryHandler.OrderRepository.Update(order);
                if (!isOrderUpdated) {
                    throw new DbContextException($"The {nameof(order)} has not been updated.");
                }

                return isOrderUpdated;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        // TODO: Implement... Abstract this out, need to be used in the API model as well 
        private bool IsValidOrderModel(OrderModel orderModel)
        {
            return true;
        }
    }
}
