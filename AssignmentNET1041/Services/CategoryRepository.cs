using AssignmentNET1041.Interfaces;
using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Services
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly DatabaseASMContext _context;

		public CategoryRepository(DatabaseASMContext context)
		{
			_context = context;
		}

		public async Task<CategoryModel> GetCategoryBySlugAsync(string slug)
		{
			return await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
		}

		public IQueryable<ProductModel> GetProductsByCategoryId(int categoryId)
		{
			return _context.Products.Where(p => p.CategoryId == categoryId);
		}

		public async Task<int> GetProductCountByCategoryIdAsync(int categoryId)
		{
			return await _context.Products.Where(p => p.CategoryId == categoryId).CountAsync();
		}
	}
}
