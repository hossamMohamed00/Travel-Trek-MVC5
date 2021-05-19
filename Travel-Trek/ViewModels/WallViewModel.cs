using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel_Trek.Models;

namespace Travel_Trek.ViewModels
{
    public class WallViewModel
    {
        public List<Post> Posts { get; set; }
        public Person User { get; set; }

    }
}