using AssignmentNET1041.Interfaces;
using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;

namespace AssignmentNET1041.Services
{
	public class CartRepository : ICartRepository
	{
		private readonly DatabaseASMContext _context;

		public CartRepository(DatabaseASMContext context)
		{
			_context = context;
		}

		public async Task<ProductModel> GetProductByIdAsync(long id)
		{
			return await _context.Products.FindAsync(id);
		}
	}
}
