using System;
using System.Data.Entity;
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
        private readonly ApplicationDbContext _dbContext;

        /* Constructor */
        public AccountController()
        {
            _dbContext = new ApplicationDbContext();
        }

        /* Override Dispose method */
        protected override void Dispose(bool disposing)
        {
            _dbContext.Dispose();
        }

        /*---------------------------*/

        // Post: Account/register
        [HttpPost]
        [Route("Account/register")]
        public JsonResult Register(Person user, HttpPostedFileBase userPhoto)
        {
            if (ModelState.IsValid)
            {
                //* Check if the given email is a unique or not
                var ExistsUser = _dbContext.Users.SingleOrDefault(u => u.Email == user.Email);

                var isTakenBefore = ExistsUser != null;

                //* If true, so the email already exists
                if (isTakenBefore)
                {
                    return Json(new { success = false, message = "Email already taken, choose another one 🤷‍♂️" }, JsonRequestBehavior.AllowGet);
                }

                if (userPhoto != null)
                {
                    var imagePath = Utilities.GetPersonImagePath(userPhoto);

                    // Save the image on the device 
                    userPhoto.SaveAs(Server.MapPath(imagePath));

                    user.Photo = imagePath;
                }

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                ModelState.Clear();


                return Json(new { success = true, message = "Welcome " + user.FirstName + " in our amazing website, We are very happy to have you ❤😃. Go ahead and login now 🕺🏻🗝" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Cannot registered you right now 😥" }, JsonRequestBehavior.AllowGet);
        }

        // Post: Account/login
        [HttpPost]
        [Route("Account/login")]
        public ActionResult Login(WallViewModel viewModel)
        {
            var loginData = viewModel.Login;

            //* Search for this user
            var user = _dbContext.Users.FirstOrDefault(u => u.Email.Equals(loginData.Email) && u.Password.Equals(loginData.Password));

            //* Check if the user exists
            if (user != null)
            {
                //* Get user cookie
                var cookie = GetAUserCookie(loginData);

                //* Add the cookie
                Response.Cookies.Add(cookie);

                //* Redirect the user to the valid path
                string Url = "/";
                if (user.UserRoleId == RoleNamesAndIds.AdminId)
                {
                    Url = "/Dashboard/";
                }

                if (user.UserRoleId == RoleNamesAndIds.AgencyId)
                {
                    Url = "/Agency/";
                }

                if (user.UserRoleId == RoleNamesAndIds.TravelerId)
                {
                    Url = "/";
                }

                return Json(new { success = true, url = Url }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //*The given user is not found
                return Json(new { success = false, message = "Invalid Email or password. 😒💔" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "An error occurred. 😐" }, JsonRequestBehavior.AllowGet);
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
                user = dbContext.Users.Include(u => u.UserRole).FirstOrDefault(u => u.Email == email);

            }

            return user;
        }

        /**
         * Prepare and get a cookie for the user
         */
        public static HttpCookie GetAUserCookie(Login loginData)
        {
            var ticket = new FormsAuthenticationTicket(loginData.Email, true, 3000);
            string encrypt = FormsAuthentication.Encrypt(ticket);

            //* Create new cookie
            var cookie =
                new HttpCookie(FormsAuthentication.FormsCookieName, encrypt)
                {
                    Expires = DateTime.Now.AddHours(3000),
                    HttpOnly = true
                };

            return cookie;
        }
    }

}
