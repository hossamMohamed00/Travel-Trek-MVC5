using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel_Trek.Models
{
    public class SavedPosts
    {
        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        [Key, Column(Order = 2)]
        public int PostId { get; set; }

        public Person User { get; set; }

        public Post Post { get; set; }
    }
}