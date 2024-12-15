using System.ComponentModel.DataAnnotations;

namespace Uniqlo.ViewModel.Auths
{
    public class ForgotPasswordVM
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
