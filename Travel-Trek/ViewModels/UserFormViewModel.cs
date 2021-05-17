using Travel_Trek.Models;

namespace Travel_Trek.ViewModels
{
    public class UserFormViewModel
    {
        public Person User { get; set; }

        public string Layout
        {
            get
            {
                if (User.UserRoleId == UserRole.AdminId)
                {
                    return "DashboardLayout.cshtml";
                }
                else if (User.UserRoleId == UserRole.AgencyId)
                {
                    return "WallLayout.cshtml";
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
                if (User.UserRoleId == UserRole.AdminId)
                {
                    return "Admin";
                }
                else if (User.UserRoleId == UserRole.AgencyId)
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
                if (User.UserRoleId == UserRole.AdminId)
                {
                    return "Dashboard";
                }
                else if (User.UserRoleId == UserRole.AgencyId)
                {
                    return "Agency";
                }
                else
                {
                    return "Travller";
                }
            }
        }
    }
}