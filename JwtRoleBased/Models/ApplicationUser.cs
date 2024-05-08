using Microsoft.AspNetCore.Identity;

namespace JwtRoleBased.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Role Role { get; set; }
    }
}
