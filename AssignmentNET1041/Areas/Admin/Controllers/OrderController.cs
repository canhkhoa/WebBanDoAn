using AssignmentNET1041.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
	{
        private readonly DatabaseASMContext _datacontext;
        public OrderController(DatabaseASMContext dataContext)
        {
            _datacontext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _datacontext.Orders.OrderByDescending(p => p.Id).ToListAsync());
        }
        public async Task<IActionResult> View(string ordercode)
        {
            var DetailsOrder = await _datacontext.OrderDetails
                                                 .Include(od => od.Product)
                                                 .Where(od => od.OrderCode == ordercode)
                                                 .ToListAsync();
            var Order = _datacontext.Orders.Where(o => o.OrderCode == ordercode).First();
            ViewBag.Status = Order.Status;
            return View(DetailsOrder);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(string ordercode, int status)
        {
            var order = await _datacontext.Orders.FirstOrDefaultAsync(o=>o.OrderCode == ordercode);
            if(order == null)
            {
                return NotFound();
            }
            order.Status = status;
            try
            {
                await _datacontext.SaveChangesAsync();
                return Ok(new { success = true, message = "Order status update successfully" });
            }catch(Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the order status");
            }
        }
		
public async Task<IActionResult> DeleteOrder(string ordercode)
{
    var order = await _datacontext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);
    if (order == null)
    {
        return NotFound();
    }

    // Tìm tất cả OrderDetails có OrderCode trùng với ordercode
    var orderDetailsToDelete = await _datacontext.OrderDetails
        .Where(od => od.OrderCode == ordercode)
        .ToListAsync();

    // Xóa các OrderDetails tìm được
    _datacontext.OrderDetails.RemoveRange(orderDetailsToDelete);

    // Xóa Order
    _datacontext.Orders.Remove(order);

    try
    {
        await _datacontext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
    catch (Exception ex)
    {
        // Log the exception for debugging
        Console.WriteLine(ex); 
        return StatusCode(500, "An error occurred while deleting the order");
    }
}

	}
}
