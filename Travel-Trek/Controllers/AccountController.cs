using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Travel_Trek.Db_Context;
using Travel_Trek.Models;
using Travel_Trek.ViewModels;

namespace Travel_Trek.Controllers
{
    public class AccountController : Controller
    {
        // Post: Account/register
        [HttpPost]
        [Route("Account/register")]
        public ActionResult Register(Person user)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext Db = new ApplicationDbContext())
                {
                    Db.Users.Add(user);
                    Db.SaveChanges();
                    ModelState.Clear();
                }
                return View("UserProfile");
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

            }
            return View("UserProfile");
        }


        //Post: Account/logout
        [Route("Account/logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Wall");

        }

    }
}