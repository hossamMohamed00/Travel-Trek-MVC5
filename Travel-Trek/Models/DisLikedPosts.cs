using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Travel_Trek.Models
{
    public class DisLikedPosts
    {
        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        [Key, Column(Order = 2)]
        public int PostId { get; set; }

        public Person User { get; set; }

        public Post Post { get; set; }
    }
}