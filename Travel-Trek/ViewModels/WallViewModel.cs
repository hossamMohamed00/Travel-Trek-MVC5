using System.Collections.Generic;
using System.Linq;
using Travel_Trek.Db_Context;
using Travel_Trek.Models;

namespace Travel_Trek.ViewModels
{
    public class WallViewModel
    {
        public List<Post> Posts { get; set; }

        public List<SavedPosts> SavedPosts { get; set; } // For saved posts page

        public List<UserQuestion> UserQuestions { get; set; } // For user questions page

        public Person User { get; set; }

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

        public Login Login { get; set; } // for login modal

        public string Layout
        {
            get
            {
                if (User.UserRoleId == RoleNamesAndIds.AdminId)
                {
                    return "DashboardLayout.cshtml";
                }
                else if (User.UserRoleId == RoleNamesAndIds.AgencyId)
                {
                    return "FactoryLayout.cshtml";
                }
                else
                {
                    return "WallLayout.cshtml";
                }
            }
        }

        public string Title
        {
            get
            {
                if (User.UserRoleId == RoleNamesAndIds.AdminId)
                {
                    return "Admin";
                }
                else if (User.UserRoleId == RoleNamesAndIds.AgencyId)
                {
                    return "Agency";
                }
                else
                {
                    return "Travller";
                }
            }
        }

        public string Controller
        {
            get
            {
                if (User.UserRoleId == RoleNamesAndIds.AdminId)
                {
                    return "Dashboard";
                }
                else if (User.UserRoleId == RoleNamesAndIds.AgencyId)
                {
                    return "Agency";
                }
                else
                {
                    return "Wall";
                }
            }
        }

        public string CurrentView { get; set; }
    }
}