using AssignmentNET1041.Models;

namespace AssignmentNET1041.Interfaces
{
	public interface ICategoryRepository
	{
		Task<CategoryModel> GetCategoryBySlugAsync(string slug);
		IQueryable<ProductModel> GetProductsByCategoryId(int categoryId);
		Task<int> GetProductCountByCategoryIdAsync(int categoryId);

	}
}
