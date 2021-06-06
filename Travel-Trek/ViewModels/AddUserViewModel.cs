using System.Collections.Generic;
using System.Linq;
using Travel_Trek.Db_Context;
using Travel_Trek.Models;

namespace Travel_Trek.ViewModels
{
    public class AddUserViewModel
    {
        public Person Person { get; set; }

        public IEnumerable<UserRole> UserRoles
        {
            get
            {
                using (ApplicationDbContext _dbContext = new ApplicationDbContext())
                {
                    var userRoles = _dbContext.UserRoles.ToList();

                    //* Get the admin role to exclude it
                    var adminRole = userRoles.Find(r => r.Name == RoleNamesAndIds.Admin);

                    // Exclude the admin role
                    userRoles.Remove(adminRole);

                    return userRoles;
                }
            }
        }
    }
}