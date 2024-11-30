using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    
    public class BrandController : Controller
    {
        private readonly DatabaseASMContext _datacontext;
        public BrandController(DatabaseASMContext dataContext)
        {
            _datacontext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _datacontext.Brands.OrderByDescending(p => p.Id).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {


            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _datacontext.Categories.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Thương hiệu đã có trong database");
                    return View(brand);
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

                _datacontext.Add(brand);
                await _datacontext.SaveChangesAsync();
                TempData["success"] = "Thêm thương hiệu thành công";
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

            return View(brand);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel brand = await _datacontext.Brands.FindAsync(Id);
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {

            var existed_brand = _datacontext.Brands.Find(brand.Id); //tim sp theo id product

            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _datacontext.Products.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Thương hiệu đã có trong database");
                    return View(brand);
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
                existed_brand.Name = brand.Name;
                existed_brand.Description = brand.Description;
                
                existed_brand.Status = brand.Status;

                //other properties
                _datacontext.Update(existed_brand);


                await _datacontext.SaveChangesAsync();
                TempData["success"] = " Sửa thương hiệu thành công";
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

            return View(brand);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await _datacontext.Brands.FindAsync(Id);
            if (brand == null)
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
            _datacontext.Brands.Remove(brand);
            await _datacontext.SaveChangesAsync();
            TempData["success"] = "Danh mục đã xóa";
            return RedirectToAction("Index");
        }

    }
}
