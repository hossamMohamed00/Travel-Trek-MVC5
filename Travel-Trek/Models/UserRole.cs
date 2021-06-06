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
    }
}