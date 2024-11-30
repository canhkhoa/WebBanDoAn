using AssignmentNET1041.Models;
using Microsoft.AspNetCore.Identity;

namespace AssignmentNET1041.Interfaces
{
	public interface IAccountService 
	{
		Task<bool> LoginAsync(string userName, string password);
		Task<bool> LogoutAsync();
		Task<IdentityResult> CreateUserAsync(AppUserModel user, string password);
		Task<IdentityResult> AddToRoleAsync(AppUserModel user, string role);
		Task<AppUserModel> FindUserByNameAsync(string userName);
		Task<bool> IsInRoleAsync(AppUserModel user, string role);
        Task<List<OrderModel>> GetOrderHistoryAsync(string userEmail);
        Task<bool> CancelOrderAsync(string ordercode);
    }
}
