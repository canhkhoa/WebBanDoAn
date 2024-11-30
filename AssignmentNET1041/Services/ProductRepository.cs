using AssignmentNET1041.Interfaces;
using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Services
{
	public class ProductRepository : IProductRepository
	{
		private readonly DatabaseASMContext _context;

		// Inject DatabaseASMContext
		public ProductRepository(DatabaseASMContext context)
		{
			_context = context;
		}

		// Lấy tất cả sản phẩm
		public async Task<List<ProductModel>> GetAllProductsAsync()
		{
			return await _context.Products
				.Include("Category")
				.Include("Brand")
				.ToListAsync();
		}

		// Lấy sản phẩm theo ID
		public async Task<ProductModel> GetProductByIdAsync(int id)
		{
			return await _context.Products
				.Where(p => p.Id == id)
				.FirstOrDefaultAsync();
		}

		// Tìm kiếm sản phẩm theo tên hoặc mô tả
		public async Task<List<ProductModel>> SearchProductsAsync(string searchTerm)
		{
			return await _context.Products
				.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
				.ToListAsync();
		}
	}
}
