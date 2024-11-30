using AssignmentNET1041.Areas.Admin.Repository;
using AssignmentNET1041.Interfaces;
using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Services
{
	public class AccountService : IAccountService
	{
		private readonly UserManager<AppUserModel> _userManager;
		private readonly SignInManager<AppUserModel> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly DatabaseASMContext _dataContext;

        public AccountService(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager, IEmailSender emailSender, DatabaseASMContext dataContext)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_dataContext = dataContext;
		}

		public async Task<bool> LoginAsync(string userName, string password)
		{
			var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);
			return result.Succeeded;
		}

		public async Task<bool> LogoutAsync()
		{
			await _signInManager.SignOutAsync();
			return true;
		}

		public async Task<IdentityResult> CreateUserAsync(AppUserModel user, string password)
		{
			return await _userManager.CreateAsync(user, password);
		}

		public async Task<IdentityResult> AddToRoleAsync(AppUserModel user, string role)
		{
			
			return await _userManager.AddToRoleAsync(user, role);
		}

		public async Task<AppUserModel> FindUserByNameAsync(string userName)
		{
			return await _userManager.FindByNameAsync(userName);
		}
		public async Task<bool> IsInRoleAsync(AppUserModel user, string role)
		{
			var result = await _userManager.IsInRoleAsync(user, "Admin");
			return result;
		}
        public async Task<List<OrderModel>> GetOrderHistoryAsync(string userEmail)
        {
            return await _dataContext.Orders
                                      .Where(od => od.UserName == userEmail)
                                      .OrderByDescending(od => od.Id)
                                      .ToListAsync();
        }

        public async Task<bool> CancelOrderAsync(string ordercode)
        {
            try
            {
                var order = await _dataContext.Orders
                                               .FirstOrDefaultAsync(o => o.OrderCode == ordercode);
                if (order != null)
                {
                    order.Status = 3;
                    _dataContext.Update(order);
                    await _dataContext.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        }
    }

