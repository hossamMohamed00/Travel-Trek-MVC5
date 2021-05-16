using System;
using System.ComponentModel.DataAnnotations;
namespace Travel_Trek.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public Person Agency { get; set; }

        [MaxLength(255)]
        [Required]
        public string Status { get; set; }

        [Required]
        [MaxLength(255)]
        public string TripTitle { get; set; }

        [Required]
        [MaxLength(255)]
        public string TripDetails { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TripDate { get; set; }

        [Required]
        [MaxLength(255)]
        public string TripDestination { get; set; }

        [Required]
        public string TripImage { get; set; }

        // Constructor
        public Post()
        {
            PostDate = DateTime.Now;
            Status = PENDING;
        }

        public static string PENDING = "Pending";
        public static string APPROVED = "Approved";
        public static string Refused = "Refused";
    }
}