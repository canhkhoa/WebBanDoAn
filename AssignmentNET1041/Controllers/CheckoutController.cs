using AssignmentNET1041.Areas.Admin.Repository;
using AssignmentNET1041.Interfaces;
using AssignmentNET1041.Models;
using AssignmentNET1041.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AssignmentNET1041.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IEmailSender _emailSender;

		public CheckoutController(IOrderRepository orderRepository, IEmailSender emailSender)
		{
			_orderRepository = orderRepository;
			_emailSender = emailSender;
		}

		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else
			{
				var orderCode = Guid.NewGuid().ToString(); // tạo mã đơn hàng
				var orderItem = new OrderModel
				{
					OrderCode = orderCode,
					UserName = userEmail,
					Status = 1, // trạng thái đơn hàng
					CreateDate = DateTime.Now
				};

				// Thêm đơn hàng vào cơ sở dữ liệu
				await _orderRepository.AddOrderAsync(orderItem);

				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				foreach (var cartItem in cartItems)
				{
					var orderDetails = new OrderDetails
					{
						UserName = userEmail,
						OrderCode = orderCode,
						ProductId = cartItem.ProductId,
						Price = cartItem.Price,
						Quantity = cartItem.Quantity
					};

					// Thêm chi tiết đơn hàng vào cơ sở dữ liệu
					await _orderRepository.AddOrderDetailsAsync(orderDetails);

					// Cập nhật số lượng sản phẩm
					await _orderRepository.UpdateProductQuantityAsync(cartItem.ProductId, cartItem.Quantity);
				}

				// Xóa giỏ hàng sau khi checkout
				HttpContext.Session.Remove("Cart");

				// Gửi email thông báo đơn hàng
				var receiver = "thanhnock12346789@gmail.com"; // Thay bằng userEmail nếu cần
				var subject = "Đặt hàng thành công";
				var message = "Đặt hàng thành công, vui lòng kiểm tra lại giỏ hàng";
				await _emailSender.SendEmailAsync(receiver, subject, message);

				TempData["success"] = "Checkout thành công, vui lòng chờ duyệt đơn hàng!";
				return RedirectToAction("History", "Account");
			}
		}
	}
}
