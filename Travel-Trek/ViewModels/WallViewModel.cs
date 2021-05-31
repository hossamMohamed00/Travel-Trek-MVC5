using System.Collections.Generic;
using Travel_Trek.Models;

namespace Travel_Trek.ViewModels
{
    public class WallViewModel
    {
        public List<Post> Posts { get; set; }

        public List<SavedPosts> SavedPosts { get; set; } // For saved posts page

        public List<UserQuestion> UserQuestions { get; set; } // For user questions page

        public Person User { get; set; }

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