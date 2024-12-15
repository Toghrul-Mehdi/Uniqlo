using System.ComponentModel.DataAnnotations;

namespace Uniqlo.ViewModel.Auths
{
    public class UserCreateVM
    {
        [MaxLength(64),Required]
        public  string Fullname { get; set; }

        [MaxLength(64), Required]
        public string Username { get; set; }

        [MaxLength(64), Required,EmailAddress]
        public string Email { get; set; }

        [MaxLength(32), Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [MaxLength(32), Required, DataType(DataType.Password),Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}
