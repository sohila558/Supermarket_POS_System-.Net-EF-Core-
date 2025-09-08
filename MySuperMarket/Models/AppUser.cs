using Microsoft.AspNetCore.Identity;

namespace MySuperMarket.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
