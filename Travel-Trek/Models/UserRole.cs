using System.ComponentModel.DataAnnotations;
namespace Travel_Trek.Models
{
    public class UserRole
    {
        [Key]
        public byte Id { get; set; }

        [Required]
        public string Name { get; set; }

        public static int AdminId = 1;
        public static int AgencyId = 2;
        public static int TravellerId = 3;
    }
}