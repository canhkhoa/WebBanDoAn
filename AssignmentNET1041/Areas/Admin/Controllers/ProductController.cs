using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly DatabaseASMContext _datacontext;
        private readonly IWebHostEnvironment _environment;
        public ProductController(DatabaseASMContext datacontext, IWebHostEnvironment environment)
        {
            _datacontext = datacontext;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _datacontext.Products.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_datacontext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_datacontext.Brands, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_datacontext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_datacontext.Brands, "Id", "Name", product.BrandId);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
                var slug = await _datacontext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(product);
                }

                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_environment.WebRootPath, "images/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }

                _datacontext.Add(product);
                await _datacontext.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm thành công";
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

            return View(product);
        }

        public async Task<IActionResult> Edit(long Id)
        {
            ProductModel product = await _datacontext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_datacontext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_datacontext.Brands, "Id", "Name", product.BrandId);

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_datacontext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_datacontext.Brands, "Id", "Name", product.BrandId);
            var existed_product = _datacontext.Products.Find(product.Id); //tim sp theo id product

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
                var slug = await _datacontext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(product);
                }

                if (product.ImageUpload != null)
                {


                    //upload new picture
                    string uploadDir = Path.Combine(_environment.WebRootPath, "images/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    //delete old picture
                    string oldfilePath = Path.Combine(uploadDir, existed_product.Image);
                    try
                    {
                        if (System.IO.File.Exists(oldfilePath))
                        {
                            System.IO.File.Delete(oldfilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occured while deleting the product image");
                    }

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    existed_product.Image = imageName;


                }
                //Update other product properties
                existed_product.Name = product.Name;
                existed_product.Description = product.Description;
                existed_product.Price = product.Price;
                existed_product.CategoryId = product.CategoryId;
                existed_product.BrandId = product.BrandId;
                //other properties
                _datacontext.Update(existed_product);


                await _datacontext.SaveChangesAsync();
                TempData["success"] = " Sửa sản phẩm thành công";
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

            return View(product);
        }

        public async Task<IActionResult> Delete(long Id)
        {
            ProductModel product = await _datacontext.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            string uploadDir = Path.Combine(_environment.WebRootPath, "images/products");
            string oldfilePath = Path.Combine(uploadDir, product.Image);
            try
            {
                if (System.IO.File.Exists(oldfilePath))
                {
                    System.IO.File.Delete(oldfilePath);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occured while deleting the product image");
            }
            _datacontext.Products.Remove(product);
            await _datacontext.SaveChangesAsync();
            TempData["success"] = "Sản phẩm đã xóa";
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public async Task<IActionResult> AddQuantity(long Id)
        {
            var productbyQuantity = await _datacontext.ProductQuantities.Where(pq=>pq.ProductId == Id).ToArrayAsync();
            ViewBag.ProductByQuantity = productbyQuantity;
            ViewBag.Id = Id;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StoreProductQuantity(ProductQuantityModel productQuantityModel)
        {
            var product = _datacontext.Products.Find(productQuantityModel.ProductId);
            if (product == null) {
                
                return NotFound(); 
            }
            product.Quantity += productQuantityModel.Quantity;
            productQuantityModel.Quantity = productQuantityModel.Quantity;
            productQuantityModel.ProductId = productQuantityModel.ProductId;
            productQuantityModel.DateCreated = DateTime.Now;

            _datacontext.Add(productQuantityModel);
            _datacontext.SaveChangesAsync();
            TempData["success"] = "Thêm số lượng thành công";
            return RedirectToAction("AddQuantity", "Product", new { Id = productQuantityModel.ProductId });


        }
    }
}
