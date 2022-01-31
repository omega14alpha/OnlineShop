using System.ComponentModel.DataAnnotations;

namespace OnlineShop.OnlineClient.Identity.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email is not specified")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Login is not specified")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is not specified")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is not specified")]
        [Compare("Password", ErrorMessage = "The password was entered incorrectly")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
