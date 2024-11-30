using System.ComponentModel.DataAnnotations;

namespace AssignmentNET1041.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Hãy nhập tên đăng nhập")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Hãy nhập Email"), EmailAddress]
		public string Email { get; set; }
		[Required(ErrorMessage = "Hãy nhập số điện thoại "), Phone]
		public string PhoneNumber { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage ="Hãy nhập mật khẩu")]
		public string Password { get; set; }
		public string Occupation { get; set; }
		public string Address { get; set; }
		


	}
}
