using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Travel_Trek.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Person> Users { get; set; }

        public static int AdminId = 1;
        public static int AgencyId = 2;
        public static int TravelerId = 3;

        public static string Admin = "Admin";
        public static string Agency = "Agency";
        public static string Traveler = "Traveler";
    }
}