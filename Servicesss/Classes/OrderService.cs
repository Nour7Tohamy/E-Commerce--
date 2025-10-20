namespace E_Commerce.Service.Classes
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderrepo;

        public OrderService(IOrderRepository orderrepo)
        {
            this._orderrepo = orderrepo;
        }


        #region Get Methods
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderrepo.GetAllAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderrepo.GetByIdAsync(orderId);
        }
        public async Task<Order?> GetUserOrderByIdAsync(string UserId, int OrderId)
        {
            return await _orderrepo.GetOrderForUserByIdAsync(UserId, OrderId);
        }
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _orderrepo.GetOrdersByUserIdAsync(userId);
        }
        #endregion

        #region Create Methods
        public async Task<OperationResultGeneric<OrderDto>> CreateOrderAsync(CreateOrderDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                return OperationResultGeneric<OrderDto>.Fail("Order must contain at least one item.");

            if (dto.Items.Any(i => i.Quantity <= 0 || i.UnitPrice <= 0))
                return OperationResultGeneric<OrderDto>.Fail("Invalid item quantity or price.");
          
            var order = new Order
            {
                PaymentMethod = dto.PaymentMethod,
                OrderType = dto.OrderType,
                DeliveryAddress = dto.DeliveryAddress,
                CustomerName = dto.CustomerName,
                UserId = dto.UserId,
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            await _orderrepo.AddAsync(order);
            await _orderrepo.SaveChangesAsync();

            var orderDto = new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                CustomerName = order.CustomerName,
                PaymentMethod = order.PaymentMethod,
                OrderType = order.OrderType,
                DeliveryAddress = order.DeliveryAddress,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };

            return OperationResultGeneric<OrderDto>.Ok(orderDto, "Order created successfully.");
        }
        #endregion

        #region Update
        public async Task<OperationResultGeneric<OrderDto>> UpdateOrderAsync(UpdateOrderDto dto)
        {
            var order = await _orderrepo.GetOrderByIdAsync(dto.OrderId);
            if (order == null || order.UserId != dto.UserId)
                return OperationResultGeneric<OrderDto>.Fail("Order not found or access denied.");

            if (!string.IsNullOrWhiteSpace(dto.PaymentMethod))
                order.PaymentMethod = dto.PaymentMethod;

            if (!string.IsNullOrWhiteSpace(dto.OrderType))
                order.OrderType = dto.OrderType;

            if (!string.IsNullOrWhiteSpace(dto.DeliveryAddress))
                order.DeliveryAddress = dto.DeliveryAddress;

            if (!string.IsNullOrWhiteSpace(dto.CustomerName))
                order.CustomerName = dto.CustomerName;

            order.OrderItems = dto.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();


            _orderrepo.Update(order);
            await _orderrepo.SaveChangesAsync();

            var updatedDto = new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                CustomerName = order.CustomerName,
                PaymentMethod = order.PaymentMethod,
                OrderType = order.OrderType,
                DeliveryAddress = order.DeliveryAddress,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };

            return OperationResultGeneric<OrderDto>.Ok(updatedDto, "Order updated successfully.");
        }


        public async Task<OperationResultGeneric<OrderDto>> UpdateOrderStatusAsync(UpdateOrderStatusDto dto)
        {
            var order = await _orderrepo.GetOrderByIdAsync(dto.OrderId);
            if (order == null)
                return OperationResultGeneric<OrderDto>.Fail("Order not found.");

            if (order.UserId != dto.UserId)
                return OperationResultGeneric<OrderDto>.Fail("Access denied. You are not allowed to update this order.");
         
            if (order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Delivered)
                return OperationResultGeneric<OrderDto>.Fail("Cannot delete an order that has been shipped or delivered.");

            bool validTransition = order.Status switch
            {
                OrderStatus.Pending => dto.Status is OrderStatus.Processing or OrderStatus.Cancelled,
                OrderStatus.Processing => dto.Status is OrderStatus.Shipped or OrderStatus.Cancelled,
                OrderStatus.Shipped => dto.Status is OrderStatus.Delivered,
                _ => false
            };

            if (!validTransition)
                return OperationResultGeneric<OrderDto>.Fail(
                    $"Invalid status transition: cannot move from {order.Status} to {dto.Status}."
                );

            order.Status = dto.Status;
            _orderrepo.Update(order);
            await _orderrepo.SaveChangesAsync();

            var orderDto = new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                CustomerName = order.CustomerName,
                PaymentMethod = order.PaymentMethod,
                OrderType = order.OrderType,
                DeliveryAddress = order.DeliveryAddress,
                Status = order.Status.ToString()
            };

            return OperationResultGeneric<OrderDto>.Ok(orderDto, "Order status updated successfully.");
        }


        #endregion

        #region Delete Methods
        public async Task<OperationResultGeneric<OrderDto>> DeleteOrderAsync(string userId, int orderId)
        {
            var order = await _orderrepo.GetOrderByIdAsync(orderId);

            if (order == null || order.UserId != userId)
                return OperationResultGeneric<OrderDto>.Fail("Order not found or access denied.");

            _orderrepo.Delete(order);
            await _orderrepo.SaveChangesAsync();

            var deletedOrderDto = new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                CustomerName = order.CustomerName,
                PaymentMethod = order.PaymentMethod,
                OrderType = order.OrderType,
                DeliveryAddress = order.DeliveryAddress
            };

            return OperationResultGeneric<OrderDto>.Ok(deletedOrderDto, "Order deleted successfully.");
        }


        #endregion

    }
}