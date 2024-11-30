using Microsoft.AspNetCore.Mvc;
using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace AssignmentNET1041.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
	{
		private readonly DatabaseASMContext _datacontext;
		public CategoryController(DatabaseASMContext dataContext)
		{
			_datacontext = dataContext;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _datacontext.Categories.OrderByDescending(p => p.Id).ToListAsync());
		}
        [HttpGet]
        public IActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
 

            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _datacontext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database");
                    return View(category);
                }

                //if (product.ImageUpload != null)
                //{
                //    string uploadDir = Path.Combine(_environment.WebRootPath, "images/products");
                //    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                //    string filePath = Path.Combine(uploadDir, imageName);

                //    FileStream fs = new FileStream(filePath, FileMode.Create);
                //    await product.ImageUpload.CopyToAsync(fs);
                //    fs.Close();
                //    product.Image = imageName;
                //}

                _datacontext.Add(category);
                await _datacontext.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục thành công";
                return RedirectToAction("Index");
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

            return View(category);
        }

        public async Task<IActionResult> Edit(int Id)
        {
            CategoryModel category = await _datacontext.Categories.FindAsync(Id);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category)
        {
      
            var existed_category = _datacontext.Categories.Find(category.Id); //tim sp theo id product

            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _datacontext.Products.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(category);
                }

                //if (product.ImageUpload != null)
                //{


                //    //upload new picture
                //    string uploadDir = Path.Combine(_environment.WebRootPath, "images/products");
                //    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                //    string filePath = Path.Combine(uploadDir, imageName);
                //    //delete old picture
                //    string oldfilePath = Path.Combine(uploadDir, existed_product.Image);
                //    try
                //    {
                //        if (System.IO.File.Exists(oldfilePath))
                //        {
                //            System.IO.File.Delete(oldfilePath);
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        ModelState.AddModelError("", "An error occured while deleting the product image");
                //    }

                //    FileStream fs = new FileStream(filePath, FileMode.Create);
                //    await product.ImageUpload.CopyToAsync(fs);
                //    fs.Close();
                //    existed_product.Image = imageName;


                //}
                //Update other product properties
                existed_category.Name = category.Name;
                existed_category.Description = category.Description;
                existed_category.Status = category.Status;
               
                //other properties
                _datacontext.Update(existed_category);


                await _datacontext.SaveChangesAsync();
                TempData["success"] = " Sửa danh mục thành công";
                return RedirectToAction("Index");
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

            return View(category);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            CategoryModel category = await _datacontext.Categories.FindAsync(Id);
            if (category == null)
            {
                return NotFound();
            }
            //string uploadDir = Path.Combine(_environment.WebRootPath, "images/products");
            //string oldfilePath = Path.Combine(uploadDir, product.Image);
            //try
            //{
            //    if (System.IO.File.Exists(oldfilePath))
            //    {
            //        System.IO.File.Delete(oldfilePath);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("", "An error occured while deleting the product image");
            //}
            _datacontext.Categories.Remove(category);
            await _datacontext.SaveChangesAsync();
            TempData["success"] = "Danh mục đã xóa thành công";
            return RedirectToAction("Index");
        }
    }
}
