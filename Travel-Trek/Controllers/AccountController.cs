﻿using System;
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
        public ActionResult Register(Person user, HttpPostedFileBase userPhoto)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext Db = new ApplicationDbContext())
                {
                    if (userPhoto != null)
                    {
                        var ImagePath = Utilities.GetPersonImagePath(userPhoto);

                        // Save the image on the device 
                        userPhoto.SaveAs(Server.MapPath(ImagePath));

                        user.Photo = ImagePath;
                    }

                    Db.Users.Add(user);
                    Db.SaveChanges();
                    ModelState.Clear();
                }

                return RedirectToAction("Index", "Wall");
            }
            return RedirectToAction("Index", "Wall");
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
                if (user.UserRoleId == RoleNamesAndIds.AdminId)
                {
                    return RedirectToAction("Index", "Dashboard");
                }

                if (user.UserRoleId == RoleNamesAndIds.AgencyId)
                {
                    return RedirectToAction("MyPosts", "Agency");
                }

                if (user.UserRoleId == RoleNamesAndIds.TravelerId)
                {

                    return RedirectToAction("Index", "Wall");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password!";
                return RedirectToAction("Index", "Wall");
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
