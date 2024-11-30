using AssignmentNET1041.Interfaces;
using AssignmentNET1041.Models;
using AssignmentNET1041.Models.ViewModels;
using AssignmentNET1041.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Controllers
{
	public class CartController : Controller
	{
		private readonly ICartRepository _cartRepository;

		public CartController(ICartRepository cartRepository)
		{
			_cartRepository = cartRepository;
		}

		public IActionResult Index()
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemViewModel cartVM = new()
			{
				CartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
			};
			return View(cartVM);
		}

		public IActionResult Checkout()
		{
			return View();
		}

		public async Task<IActionResult> Add(long Id)
		{
			ProductModel product = await _cartRepository.GetProductByIdAsync(Id);

			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItem == null)
			{
				cart.Add(new CartItemModel(product));
			}
			else
			{
				if (product.Quantity > cartItem.Quantity)
				{
					cartItem.Quantity += 1;
					TempData["success"] = "Thêm sản phẩm vào giỏ hàng thành công";
				}
				else
				{
					cartItem.Quantity = product.Quantity;
					TempData["success"] = "Số lượng vượt quá tồn kho";
				}
			}
			HttpContext.Session.SetJson("Cart", cart);

			return Redirect(Request.Headers["Referer"].ToString());
		}

		public async Task<IActionResult> Decrease(long Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItem.Quantity > 1)
			{
				--cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}

			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

			TempData["success"] = "Giảm số lượng sản phẩm thành công";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Increase(long Id)
		{
			ProductModel product = await _cartRepository.GetProductByIdAsync(Id);
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItem.Quantity > 0 && product.Quantity > cartItem.Quantity)
			{
				++cartItem.Quantity;
				TempData["success"] = "Tăng số lượng thành công";
			}
			else
			{
				cartItem.Quantity = product.Quantity;
				TempData["success"] = "Số lượng vượt quá tồn kho";
			}

			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Remove(long Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			cart.RemoveAll(p => p.ProductId == Id);

			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

			TempData["success"] = "Xóa sản phẩm thành công";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Clear(long Id)
		{
			HttpContext.Session.Remove("Cart");
			TempData["success"] = "Làm mới giỏ hàng";
			return RedirectToAction("Index");
		}
	}
}
