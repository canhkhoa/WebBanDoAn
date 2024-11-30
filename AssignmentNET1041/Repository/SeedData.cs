using AssignmentNET1041.Models;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Repository
{
	public class SeedData
	{
		public static void SeedingData(DatabaseASMContext _context)
		{
			_context.Database.Migrate();
			//if (!_context.Products.Any())
			//{
			//	CategoryModel macbook = new CategoryModel { Name = "Macbook", Slug = "Macbook", Description = "Macbook is Large Product in the World", Status = 1 };
			//	CategoryModel pc = new CategoryModel { Name = "PC", Slug = "PC", Description = "PC is Large Product in the World", Status = 1 };

			//	BrandModel apple = new BrandModel { Name = "Apple", Slug = "Apple", Description = "Apple is Large Brand in the World", Status = 1 };
			//	BrandModel samsung = new BrandModel { Name = "Samsung", Slug = "Samsung", Description = "Samsung is Large Brand in the World", Status = 1 };

			//	_context.Products.AddRange(
			//		new ProductModel { Name = "Macbook", Slug = "Macbook", Description = "Macboook is the best", Image = "1.jpg", Category = macbook, Brand = apple, Price = 1233 },
			//		new ProductModel { Name = "PC", Slug = "PC", Description = "PC is the best", Image = "2.jpg", Category = pc, Brand = samsung, Price = 1233 }
			//	);
			//	_context.SaveChanges();
			//}
		}
	}
}
