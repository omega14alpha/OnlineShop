using Microsoft.AspNetCore.Identity;

namespace OnlineShop.OnlineClient.Identity.Models
{
    public class UserModel : IdentityUser
    {
        public string Password { get; set; }

        public string Login { get; set; }
    }
}
