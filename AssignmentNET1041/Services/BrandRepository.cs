using AssignmentNET1041.Interfaces;
using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Services
{
	public class BrandRepository : IBrandRepository
	{
		private readonly DatabaseASMContext _context;

		public BrandRepository(DatabaseASMContext context)
		{
			_context = context;
		}

		public async Task<BrandModel> GetBrandBySlugAsync(string slug)
		{
			return await _context.Brands.FirstOrDefaultAsync(b => b.Slug == slug);
		}

		public IQueryable<ProductModel> GetProductsByBrandId(int brandId)
		{
			return _context.Products.Where(p => p.BrandId == brandId);
		}

		public async Task<int> GetProductCountByBrandIdAsync(int brandId)
		{
			return await _context.Products.Where(p => p.BrandId == brandId).CountAsync();
		}
	}
}
