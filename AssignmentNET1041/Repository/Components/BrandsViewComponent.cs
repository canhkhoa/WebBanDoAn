using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssignmentNET1041.Repository.Components
{
	public class BrandsViewComponent : ViewComponent
	{
		private readonly DatabaseASMContext _dbContext;
		public BrandsViewComponent(DatabaseASMContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IViewComponentResult> InvokeAsync() => View(await _dbContext.Brands.ToListAsync());
	}
}
