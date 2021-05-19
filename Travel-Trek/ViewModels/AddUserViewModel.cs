using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel_Trek.Models;

namespace Travel_Trek.ViewModels
{
    public class AddUserViewModel
    {
        public Person Person { get; set; }

        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}