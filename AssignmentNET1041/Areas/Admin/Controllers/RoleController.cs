using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly DatabaseASMContext _datacontext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(DatabaseASMContext dataContext, RoleManager<IdentityRole> roleManager)
        {
            _datacontext = dataContext;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _datacontext.Roles.OrderByDescending(p => p.Id).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole model)
        {

            if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
                TempData["success"] = "Thêm vai trò thành công";
            }
            return Redirect("Index");


        }
        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
			if (string.IsNullOrEmpty(Id))
			{
				return NotFound();
			}

			// Lấy danh sách Id của các role mặc định (Admin và Customer)
			var defaultRoleIds = new List<string> {
				"29eafb2f-bf4c-49eb-b91a-0ba806450f82", // Id của role "Admin"
                "4125d29d-3fcb-4056-9335-c58300d9dbe5"  // Id của role "Customer"
            };

			// Kiểm tra xem Id có thuộc danh sách các role mặc định hay không
			if (defaultRoleIds.Contains(Id))
			{
				TempData["error"] = "Không thể xóa role mặc định.";
				return RedirectToAction("Index");
			}

			var role = await _roleManager.FindByIdAsync(Id);
			if (role == null)
			{
				return NotFound();
			}

			try
			{
				await _roleManager.DeleteAsync(role);
				TempData["success"] = "Xóa vai trò thành công";
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "An error occured while deleting the role");
			}
			return RedirectToAction("Index");
		}
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(Id);
            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, IdentityRole model)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(Id);
                if (role == null)
                {
                    return NotFound();
                }
                role.Name = model.Name;
                try
                {
                    await _roleManager.UpdateAsync(role);
                    TempData["success"] = "Sửa vai trò thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occured while updating the role");
                }
                
            }
            return View(model ?? new IdentityRole { Id = Id });
        }
    }
}
