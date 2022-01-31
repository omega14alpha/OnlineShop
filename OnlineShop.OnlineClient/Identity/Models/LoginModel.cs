using System.ComponentModel.DataAnnotations;

namespace OnlineShop.OnlineClient.Identity.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is not specified")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is not specified")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
