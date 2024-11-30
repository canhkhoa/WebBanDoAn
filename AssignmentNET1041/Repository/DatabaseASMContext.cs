using AssignmentNET1041.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Repository
{
	public class DatabaseASMContext : IdentityDbContext<AppUserModel>
	{
		public DatabaseASMContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<OrderModel> Orders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
		public DbSet<ProductQuantityModel> ProductQuantities { get; set; }
		

	}
}
