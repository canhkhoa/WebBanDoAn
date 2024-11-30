using AssignmentNET1041.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AssignmentNET1041.Models.ViewModels;
using AssignmentNET1041.Interfaces;
using System.Security.Claims;

namespace AssignmentNET1041.Controllers
{
    public class AccountController : Controller
    {
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if (ModelState.IsValid)
			{
				if (await _accountService.LoginAsync(loginVM.UserName, loginVM.Password))
				{
					var user = await _accountService.FindUserByNameAsync(loginVM.UserName);
					if (user != null && await _accountService.IsInRoleAsync(user, "Admin"))
					{
						return Redirect("/Admin/Product");
					}
					return Redirect(loginVM.ReturnUrl ?? "/");
				}
				ModelState.AddModelError("", "Kiểm tra lại username hoặc password");
			}
			return View(loginVM);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserModel user)
		{
			if (ModelState.IsValid)
			{
				AppUserModel newUser = new AppUserModel
				{
					UserName = user.UserName,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					Occupation = user.Occupation,
					Address = user.Address
				};
				var result = await _accountService.CreateUserAsync(newUser, user.Password);
				if (result.Succeeded)
				{
					await _accountService.AddToRoleAsync(newUser, "Customer");
					TempData["success"] = "Tạo user thành công";
					return Redirect("/Account/Login");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return View(user);
		}

		public async Task<IActionResult> Logout(string returnUrl = "/")
		{
			await _accountService.LogoutAsync();
			return Redirect(returnUrl);
		}
        public async Task<IActionResult> History()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _accountService.GetOrderHistoryAsync(userEmail);
            return View(orders);
        }

        // Cancel Order
        public async Task<IActionResult> CancelOrder(string ordercode)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var success = await _accountService.CancelOrderAsync(ordercode);
            if (!success)
            {
                return BadRequest("An error occurred while canceling the order.");
            }

            return RedirectToAction("History");
        }
    }
}
