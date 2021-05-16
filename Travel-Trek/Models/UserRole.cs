using System.ComponentModel.DataAnnotations;
namespace Travel_Trek.Models
{
    public class UserRole
    {
        [Key]
        public byte Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}