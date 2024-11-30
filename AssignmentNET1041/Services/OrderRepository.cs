using AssignmentNET1041.Interfaces;
using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Services
{
	public class OrderRepository : IOrderRepository
	{
		private readonly DatabaseASMContext _context;

		public OrderRepository(DatabaseASMContext context)
		{
			_context = context;
		}

		public async Task AddOrderAsync(OrderModel order)
		{
			await _context.AddAsync(order);
			await _context.SaveChangesAsync();
		}

		public async Task AddOrderDetailsAsync(OrderDetails orderDetails)
		{
			await _context.AddAsync(orderDetails);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateProductQuantityAsync(long productId, int quantity)
		{
			var product = await _context.Products.Where(p => p.Id == productId).FirstOrDefaultAsync();
			if (product != null)
			{
				product.Quantity -= quantity;
				product.Sold += quantity;
				_context.Update(product);
				await _context.SaveChangesAsync();
			}
		}
	}
}
