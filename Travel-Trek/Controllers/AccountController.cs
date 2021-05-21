using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Travel_Trek.Db_Context;
using Travel_Trek.Helpers;
using Travel_Trek.Models;
using Travel_Trek.ViewModels;

namespace Travel_Trek.Controllers
{
    public class AccountController : Controller
    {
        ApplicationDbContext Db;

        /* Constructor */
        public AccountController()
        {
            Db = new ApplicationDbContext();
        }

        /* Override Dispose method */
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
        }

        /*---------------------------*/

        // Post: Account/register
        [HttpPost]
        [Route("Account/register")]
        public ActionResult Register(Person user, HttpPostedFileBase userPhoto)
        {
            if (ModelState.IsValid)
            {
                var ImagePath = Utilities.GetPersonImagePath(userPhoto);

                // Save the image on the device 
                userPhoto.SaveAs(Server.MapPath(ImagePath));

                user.Photo = ImagePath;

                using (ApplicationDbContext Db = new ApplicationDbContext())
                {
                    Db.Users.Add(user);
                    Db.SaveChanges();
                    ModelState.Clear();
                }

                //* Prepare the login data
                var viewModel = new WallViewModel { Login = new Login(user.Email, user.Password) };

                return RedirectToAction("Login", viewModel);
            }
            return RedirectToAction("Index", "Wall");
        }

        // Post: Account/login
        [HttpPost]
        [Route("Account/login")]
        public ActionResult Login(WallViewModel viewModel)
        {
            var loginData = viewModel.Login;

            using (ApplicationDbContext Db = new ApplicationDbContext())
            {
                //* Search for this user
                var user = Db.Users.FirstOrDefault(u => u.Email.Equals(loginData.Email) && u.Password.Equals(loginData.Password));

                //* Check if the user exists
                if (user != null)
                {
                    var Ticket = new FormsAuthenticationTicket(loginData.Email, true, 3000);
                    string Encrypt = FormsAuthentication.Encrypt(Ticket);

                    //* Create new cookie
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, Encrypt);
                    cookie.Expires = DateTime.Now.AddHours(3000);
                    cookie.HttpOnly = true;

                    //* Add the cookie
                    Response.Cookies.Add(cookie);

                    if (user.UserRoleId == UserRole.AdminId)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }

                    if (user.UserRoleId == UserRole.AgencyId)
                    {
                        return RedirectToAction("Index", "Agency");
                    }

                    if (user.UserRoleId == UserRole.TravelerId)
                    {

                        return RedirectToAction("Index", "Wall");
                    }
                }
                else
                {
                    //* Add Code Here
                    ViewBag.LoginStatus = "Invalid email or password";
                    return RedirectToAction("Index", "Wall");
                }

            }
            return RedirectToAction("Index", "Wall");
        }


        //Post: Account/logout
        [Route("Account/logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Wall");

        }


        /* Helper Methods */
        public static Person GetUserFromEmail(string email)
        {
            Person user;
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                user = dbContext.Users.Include("UserRole").FirstOrDefault(u => u.Email == email);

            }

            return user;
        }
    }

}
