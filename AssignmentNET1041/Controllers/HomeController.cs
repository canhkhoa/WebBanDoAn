using AssignmentNET1041.Interfaces;
using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AssignmentNET1041.Controllers
{
	public class HomeController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly ILogger<HomeController> _logger;

		// Constructor với Dependency Injection cho ProductRepository
		public HomeController(ILogger<HomeController> logger, IProductRepository productRepository)
		{
			_logger = logger;
			_productRepository = productRepository;
		}

		public async Task<IActionResult> Index(string sort_by = "")
		{
			// Lấy tất cả sản phẩm từ repository một cách bất đồng bộ
			var products = await _productRepository.GetAllProductsAsync();

			// Áp dụng sắp xếp cho sản phẩm
			if (sort_by == "price_increase")
			{
				products = products.OrderBy(p => p.Price).ToList();
			}
			else if (sort_by == "price_decrease")
			{
				products = products.OrderByDescending(p => p.Price).ToList();
			}
			else if (sort_by == "price_newest")
			{
				products = products.OrderByDescending(p => p.Id).ToList();
			}
			else if (sort_by == "price_oldest")
			{
				products = products.OrderBy(p => p.Id).ToList();
			}

			return View(products);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(int statuscode)
		{
			if (statuscode == 404)
			{
				return View("NotFound");
			}
			else
			{
				return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			}
		}
	}
}
