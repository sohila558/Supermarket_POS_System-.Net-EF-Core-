using Microsoft.AspNetCore.Identity;

namespace MySuperMarket.DTOs
{
    public class CreateRolesDTO : IdentityUser
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
