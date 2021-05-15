using System;
using System.ComponentModel.DataAnnotations;
namespace Travel_Trek.Models
{
      public class Post
      {
            [Required]
            public int Id { get; set; }

            [Required]
            [MaxLength(255)]
            public Person Agency { get; set; }

            [MaxLength(255)]
            public string Status { get; set; } = "Pending";

            [Required]
            [MaxLength(255)]
            public string TripTitle { get; set; }

            [Required]
            [MaxLength(255)]
            public string TripDetails { get; set; }

            public DateTime PostDate { get; set; } = DateTime.Now;

            [Required]
            public DateTime TripDate { get; set; }

            [Required]
            [MaxLength(255)]
            public string TripDestination { get; set; }

            [Required]
            public string TripImage { get; set; }
      }
}