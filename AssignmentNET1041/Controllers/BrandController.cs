using AssignmentNET1041.Interfaces;
using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Controllers
{
    public class BrandController : Controller
    {
		private readonly IBrandRepository _brandRepository;

		// Constructor sử dụng Dependency Injection
		public BrandController(IBrandRepository brandRepository)
		{
			_brandRepository = brandRepository;
		}

		public async Task<IActionResult> Index(string Slug = "", string sort_by = "")
		{
			// Lấy thông tin thương hiệu từ repository
			BrandModel brand = await _brandRepository.GetBrandBySlugAsync(Slug);
			if (brand == null) return RedirectToAction("Index");

			// Lấy danh sách sản phẩm theo BrandId từ repository
			IQueryable<ProductModel> productByBrand = _brandRepository.GetProductsByBrandId(brand.Id);

			// Lấy số lượng sản phẩm trong thương hiệu
			var count = await _brandRepository.GetProductCountByBrandIdAsync(brand.Id);
			if (count > 0)
			{
				// Sắp xếp sản phẩm theo lựa chọn
				if (sort_by == "price_increase")
				{
					productByBrand = productByBrand.OrderBy(p => p.Price);
				}
				else if (sort_by == "price_decrease")
				{
					productByBrand = productByBrand.OrderByDescending(p => p.Price);
				}
				else if (sort_by == "price_newest")
				{
					productByBrand = productByBrand.OrderByDescending(p => p.Id);
				}
				else if (sort_by == "price_oldest")
				{
					productByBrand = productByBrand.OrderBy(p => p.Id);
				}
			}

			return View(await productByBrand.ToListAsync());
		}
	}
}

