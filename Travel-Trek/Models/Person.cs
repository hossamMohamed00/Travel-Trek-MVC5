using System.ComponentModel.DataAnnotations;
namespace Travel_Trek.Models
{
    public class Person
    {
        [Key]
        public byte Id { get; set; }

        [MaxLength(255)]
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(255)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(255)]
        [Required]
        public string Password { get; set; }

        [MaxLength(255)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string Photo { get; set; }
        [Display(Name = "Role")]
        public UserRole UserRole { get; set; }

        [Display(Name = "User Role")]
        [Required]
        public byte UserRoleId { get; set; }
    }
}