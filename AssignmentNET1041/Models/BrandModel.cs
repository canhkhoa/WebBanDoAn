using System.ComponentModel.DataAnnotations;

namespace AssignmentNET1041.Models
{
	public class BrandModel
	{
		[Key]
		public int Id { get; set; }
		[Required( ErrorMessage = "Yêu cầu nhập tên thương hiệu ")]

		public string Name { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập mô tả danh mục ")]
		public string Description { get; set; }
		public string Slug { get; set; }
		public int Status { get; set; }
	}
}
