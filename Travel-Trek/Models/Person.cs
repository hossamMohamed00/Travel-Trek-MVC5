using System.ComponentModel.DataAnnotations;
namespace Travel_Trek.Models
{
      public class Person
      {
            [Key]
            public byte Id { get; set; }

            [Required]
            [MaxLength(255)]
            public string FirstName { get; set; }

            [MaxLength(255)]
            public string LastName { get; set; } = "";

            [MaxLength(255)]
            [Required]
            public string Email { get; set; }

            [Required]
            [MaxLength(255)]
            public string Password { get; set; }

            [MaxLength(255)]
            public string PhoneNumber { get; set; }

            public string Photo { get; set; }

            [Required]
            public UserRoles UserRole { get; set; }
      }
}