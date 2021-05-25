﻿using System.ComponentModel.DataAnnotations;
namespace Travel_Trek.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(255)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [MaxLength(255)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        public string Photo { get; set; }

        [Display(Name = "Role")]
        public UserRole UserRole { get; set; }

        [Display(Name = "User Role")]
        [Required]
        public int UserRoleId { get; set; }



        // Constructor
        public Person()
        {
            UserRoleId = RoleNamesAndIds.TravelerId;
        }
    }
}