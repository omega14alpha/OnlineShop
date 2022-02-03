using Microsoft.AspNetCore.Identity;

namespace OnlineShop.OnlineClient.Identity.Models
{
    public class UserModel : IdentityUser
    {
        public string Login { get; set; }
    }
}
