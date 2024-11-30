using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization.DataContracts;

namespace AssignmentNET1041.Repository.Components
{
	public class CategoriesViewComponent : ViewComponent
	{
		private readonly DatabaseASMContext _dbContext;
		public CategoriesViewComponent(DatabaseASMContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IViewComponentResult> InvokeAsync() => View(await _dbContext.Categories.ToListAsync());
	}
}
