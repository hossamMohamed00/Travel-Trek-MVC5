using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel_Trek.Models
{
    public class UserQuestion
    {
        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        [Key, Column(Order = 2)]
        public int PostId { get; set; }

        public Person User { get; set; }

        public Post Post { get; set; }

        [DataType(DataType.Text)]
        [Required]
        [MaxLength(Int32.MaxValue)]
        public string Question { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(Int32.MaxValue)]
        public string Answer { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Status { get; set; }

        /* Static properties */
        public static string Closed = "Closed";
        public static string Open = "Open";

        public UserQuestion()
        {
            Date = DateTime.Now;
            Status = Open;
        }
    }
}