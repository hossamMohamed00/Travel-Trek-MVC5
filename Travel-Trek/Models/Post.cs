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

        [Required]
        public int AgencyId { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Title")]
        public string TripTitle { get; set; }

        [MaxLength(255)]
        [Display(Name = "Details")]
        public string TripDetails { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Trip Date")]
        public DateTime TripDate { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Destination")]
        public string TripDestination { get; set; }

        [Display(Name = "Image")]
        [FileExtensions(Extensions = "jpg,jpeg,png")]
        [DataType(DataType.ImageUrl)]
        public string TripImage { get; set; }

        [MaxLength(255)]
        [Required]
        public string Status { get; set; }

        [Range(0, int.MaxValue)]
        public int Likes { get; set; }

        [Range(0, int.MaxValue)]
        public int DisLikes { get; set; }

        [DataType(DataType.Text)]
        public string RefuseMessage { get; set; }

        // Constructor
        public Post()
        {
            //* Set the default values
            PostDate = DateTime.Now;
            Status = PENDING;
        }

        public static string PENDING = "Pending";
        public static string APPROVED = "Approved";
        public static string REFUSED = "Refused";
    }
}