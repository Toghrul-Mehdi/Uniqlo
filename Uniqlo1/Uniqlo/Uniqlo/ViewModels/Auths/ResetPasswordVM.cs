using System.ComponentModel.DataAnnotations;

namespace Uniqlo.ViewModel.Auths
{
    public class ResetPasswordVM
    {
        public string Token { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Sifreler uygun deil")]
        public string ConfirmPassword { get; set; }
    }
}
