using Microsoft.AspNetCore.Identity;

namespace AssignmentNET1041.Models
{
    public class AppUserModel : IdentityUser
    {
		public string Address { get; set; }
		public string Occupation {  get; set; }
        public string RoleId { get; set; }
        
    }
}
