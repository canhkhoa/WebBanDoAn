using AssignmentNET1041.Models;

namespace AssignmentNET1041.Interfaces
{
	public interface IOrderRepository
	{
		Task AddOrderAsync(OrderModel order);
		Task AddOrderDetailsAsync(OrderDetails orderDetails);
		Task UpdateProductQuantityAsync(long productId, int quantity);
	}
}
