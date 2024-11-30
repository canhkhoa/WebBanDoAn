using AssignmentNET1041.Models;

namespace AssignmentNET1041.Interfaces
{
	public interface ICartRepository
	{
		Task<ProductModel> GetProductByIdAsync(long id);
	}
}
