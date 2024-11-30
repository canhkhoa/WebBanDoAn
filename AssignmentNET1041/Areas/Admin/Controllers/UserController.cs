using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AssignmentNET1041.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
	{
		private readonly UserManager<AppUserModel> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly DatabaseASMContext _dataContext;
		
		public UserController(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager, DatabaseASMContext dataContext)
		{
			
			_userManager = userManager;
			_roleManager = roleManager;
			_dataContext = dataContext;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var userWithRoles = await (from u in _dataContext.Users
									   join ur in _dataContext.UserRoles on u.Id equals ur.UserId
									   join r in _dataContext.Roles on ur.RoleId equals r.Id
									   select new {User = u, RoleName = r.Name}).ToListAsync();
			return View(userWithRoles);
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			ViewBag.Roles = new SelectList(roles, "Id", "Name");
			return View(new AppUserModel()); 
		}
        
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppUserModel user)
        {
			if (ModelState.IsValid)
			{
				var createUserResult = await _userManager.CreateAsync(user, user.PasswordHash);
				if (createUserResult.Succeeded)
				{
					var createUser = await _userManager.FindByEmailAsync(user.Email);//tim user dua vao email
					var userId = createUser.Id;//lay user id
					var role = _roleManager.FindByIdAsync(user.RoleId);//lay role Id

					//gan quyen
					var addToRoleResult = await _userManager.AddToRoleAsync(createUser, role.Result.Name);
					if (!addToRoleResult.Succeeded)
					{
                        foreach (var error in createUserResult.Errors)
                        {
							ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
					return RedirectToAction("Index", "User");
				}
				else
				{
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(user);
				}
			}
			else
			{
                TempData["error"] = "Có một số thứ bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(user);
        }
		private void AddIdentityErrors(IdentityResult result)
		{
			foreach(var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, AppUserModel user)
		{
			var existingUser = await _userManager.FindByIdAsync(Id);
			if (existingUser == null)
			{
				return NotFound();
			}
			if(ModelState.IsValid)
			{
				existingUser.UserName = user.UserName;
				existingUser.Email = user.Email;	
				existingUser.PhoneNumber = user.PhoneNumber;
				existingUser.RoleId = user.RoleId;
                var currentRoles = await _userManager.GetRolesAsync(existingUser);
                await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                // Xóa role cũ
                var newRole = await _roleManager.FindByIdAsync(user.RoleId);
                await _userManager.AddToRoleAsync(existingUser, newRole.Name);
                var updateUserResult = await _userManager.UpdateAsync(existingUser);
				if(updateUserResult.Succeeded)
				{
					return RedirectToAction("Index", "User");
				}
				else
				{
					AddIdentityErrors(updateUserResult);
					return View(existingUser);
				}
			}
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

			TempData["error"] = "Model validation failed";
			var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
			string errorMessage = string.Join("/n", errors);
			return View(existingUser);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
			if (string.IsNullOrEmpty(Id))
			{
				return NotFound();
			}
			var user= await _userManager.FindByIdAsync(Id);
			if(user == null)
			{
				return NotFound();
			}

			// Kiểm tra xem user có phải là admin không
			if (await _userManager.IsInRoleAsync(user, "Admin"))
			{
				TempData["error"] = "Không thể xóa người dùng là admin.";
				return RedirectToAction("Index");
			}
			var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
				return View("Error");
            }
            TempData["error"] = "Người dùng đã xóa thành công";
            return RedirectToAction("Index");
        }
    }
}
