﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Travel_Trek.Db_Context;
using Travel_Trek.Models;
using Travel_Trek.ViewModels;

namespace Travel_Trek.Controllers
{
    public class DashboardController : Controller
    {
        ApplicationDbContext Db;

        /* Constructor */
        public DashboardController()
        {
            Db = new ApplicationDbContext();
        }

        /* Override Dispose method */
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
        }

        /* Dashboard Actions */

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        // GET: Dashboard/Users
        [Route("Dashboard/Users")]
        public ActionResult AllUsers()
        {
            var users = GetAllUsers();

            return View(users);
        }

        // Get: User/Profile
        [Route("Dashboard/Admin/Profile")]
        public ActionResult Profile()
        {
            var viewModel = GetUserFormViewModel();

            return View("UserProfile", viewModel);
        }

        [Route("Dashboard/Admin/Profile/Edit")]
        public ActionResult Edit()
        {
            var viewModel = GetUserFormViewModel();

            return View("UserProfileEdit", viewModel);
        }


        [HttpPost]
        [Route("Dashboard/Delete")]
        public JsonResult Delete(int id)
        {
            var user = Db.Users.Single(c => c.Id == id);
            Db.Users.Remove(user);
            Db.SaveChanges();
            return Json(new { success = true, message = "User deleted successfully" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Dashboard/admin/profile/Save")]
        public ActionResult Save(UserFormViewModel ViewModel)
        {
            var person = ViewModel.User;

            if (!ModelState.IsValid)
            {
                var admin = Db.Users.Include("UserRole").SingleOrDefault(u => u.Id == 1); // Need Edit later
                var viewModel = new UserFormViewModel
                {
                    User = admin
                };
                return View("UserProfileEdit", viewModel);
            }

            var adminInDb = Db.Users.SingleOrDefault(m => m.Id == person.Id);
            adminInDb.FirstName = person.FirstName;
            adminInDb.LastName = person.LastName;
            adminInDb.Password = person.Password; // Need hash later
            adminInDb.PhoneNumber = person.PhoneNumber;
            //adminInDb.Photo = person.Photo; // Need change later

            Db.SaveChanges();
            return RedirectToAction("Profile", "Dashboard");

        }

        // Get: Dashboard/Posts/Pending
        [Route("Dashboard/Posts/Pending")]
        public ActionResult PendingPosts()
        {
            var PendingPosts = GetAllPosts().Where(y => y.Status == Post.PENDING);

            return View(PendingPosts);
        }

        // Get: Dashboard/Posts
        [Route("Dashboard/Posts")]
        public ActionResult AllPosts()
        {
            var allPosts = GetAllPosts();

            return View(allPosts);
        }

        public JsonResult DeletePost(int id)
        {
            //* Delete Post
            var post = Db.Posts.Single(p => p.Id == id);
            Db.Posts.Attach(post);
            Db.Posts.Remove(post);
            Db.SaveChanges();

            return Json(new { success = true, message = "Post deleted successfully!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApprovePost(int id)
        {
            var post = Db.Posts.Single(p => p.Id == id);
            post.Status = Post.APPROVED;
            try
            {
                Db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new { success = true, message = "The Trip Post Now On the Wall!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RefusePost(int id, string refuseMessage)
        {
            var post = Db.Posts.Single(p => p.Id == id);
            post.Status = Post.REFUSED;
            post.RefuseMessage = refuseMessage;
            Db.SaveChanges();

            return Json(new { success = true, message = "Refuse the trip post done successfully!" }, JsonRequestBehavior.AllowGet);
        }


        /* Helper Methods */

        // Return all users (agencies and travelers)
        public IEnumerable<Person> GetAllUsers()
        {
            var users = Db.Users.Include("UserRole").ToList();
            return users;
        }

        // Return all users (agencies and travelers)
        public IEnumerable<Post> GetAllPosts()
        {
            var posts = Db.Posts.ToList();
            return posts;
        }

        public UserFormViewModel GetUserFormViewModel()
        {
            var admin = Db.Users.Include("UserRole").SingleOrDefault(u => u.Id == 1); // Need Edit later

            var viewModel = new UserFormViewModel
            {
                User = admin
            };

            return viewModel;
        }
    }
}