using AssignmentNET1041.Interfaces;
using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Controllers
{
    public class CategoryController : Controller
    {
		private readonly ICategoryRepository _categoryRepository;

		public CategoryController(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public async Task<IActionResult> Index(string Slug = "", string sort_by = "")
		{
			// Lấy Category dựa trên Slug
			CategoryModel category = await _categoryRepository.GetCategoryBySlugAsync(Slug);
			if (category == null) return RedirectToAction("Index");

			// Lấy danh sách sản phẩm theo CategoryId
			IQueryable<ProductModel> productByCategory = _categoryRepository.GetProductsByCategoryId(category.Id);

			// Lấy số lượng sản phẩm trong category
			var count = await _categoryRepository.GetProductCountByCategoryIdAsync(category.Id);
			if (count > 0)
			{
				// Sắp xếp sản phẩm theo lựa chọn
				if (sort_by == "price_increase")
				{
					productByCategory = productByCategory.OrderBy(p => p.Price);
				}
				else if (sort_by == "price_decrease")
				{
					productByCategory = productByCategory.OrderByDescending(p => p.Price);
				}
				else if (sort_by == "price_newest")
				{
					productByCategory = productByCategory.OrderByDescending(p => p.Id);
				}
				else if (sort_by == "price_oldest")
				{
					productByCategory = productByCategory.OrderBy(p => p.Id);
				}
			}
			return View(await productByCategory.ToListAsync());
		}
	}
}
