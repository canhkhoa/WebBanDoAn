using AssignmentNET1041.Models;

namespace AssignmentNET1041.Interfaces
{
	public interface IProductRepository
	{
		Task<List<ProductModel>> GetAllProductsAsync();
		Task<ProductModel> GetProductByIdAsync(int id);
		Task<List<ProductModel>> SearchProductsAsync(string searchTerm);
	}
}
