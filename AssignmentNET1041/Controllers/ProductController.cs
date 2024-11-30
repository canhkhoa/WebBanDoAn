using AssignmentNET1041.Interfaces;
using AssignmentNET1041.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductRepository _productRepository;

		public ProductController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Details(int id)
		{
			if (id == 0)
				return RedirectToAction("Index");

			var productByID = await _productRepository.GetProductByIdAsync(id);
			if (productByID == null)
				return NotFound();

			return View(productByID);
		}

		public async Task<IActionResult> Search(string searchTerm)
		{
			var products = await _productRepository.SearchProductsAsync(searchTerm);
			ViewBag.Keyword = searchTerm;
			return View(products);
		}
	}
}
