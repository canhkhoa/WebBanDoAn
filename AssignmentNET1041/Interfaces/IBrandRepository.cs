using AssignmentNET1041.Models;

namespace AssignmentNET1041.Interfaces
{
	public interface IBrandRepository
	{
		Task<BrandModel> GetBrandBySlugAsync(string slug);
		IQueryable<ProductModel> GetProductsByBrandId(int brandId);
		Task<int> GetProductCountByBrandIdAsync(int brandId);
	}
}
