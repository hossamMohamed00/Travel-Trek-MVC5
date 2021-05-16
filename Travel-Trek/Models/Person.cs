using System.ComponentModel.DataAnnotations;
namespace Travel_Trek.Models
{
    public class Person
    {
        [Key]
        public byte Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(255)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = "";

        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [MaxLength(255)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string Photo { get; set; }

        public UserRole UserRole { get; set; }
    }
}