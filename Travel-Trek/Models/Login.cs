using System.ComponentModel.DataAnnotations;

namespace Travel_Trek.Models
{
    public class Login
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public Login()
        {

        }

        public Login(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}