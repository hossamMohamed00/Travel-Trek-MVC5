using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
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
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Dashboard/Users
        [Authorize(Roles = "Admin")]
        [Route("Dashboard/users")]
        public ActionResult AllUsers()
        {
            var users = GetAllUsers();

            return View(users);
        }

        [Route("Dashboard/users/create")]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUser()
        {
            var userRoles = Db.UserRoles.ToList();

            //* Get the admin role to exclude it
            var adminRole = userRoles.Find(r => r.Name == UserRole.Admin);

            // Exclude the admin role
            userRoles.Remove(adminRole);

            // Initialize the viewModel
            var viewModel = new AddUserViewModel
            {
                UserRoles = userRoles
            };

            return View(viewModel);
        }

        //* Create User Logic Will be here

        [HttpPost]
        [Route("Dashboard/users/delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //* Get the user and remove him
            var user = Db.Users.Single(c => c.Id == id);
            Db.Users.Remove(user);
            Db.SaveChanges();
            return Json(new { success = true, message = "User deleted successfully" }, JsonRequestBehavior.AllowGet);
        }

        // Get: User/Profile
        [Route("Dashboard/admin/profile")]
        [Authorize(Roles = "Admin")]
        public ActionResult Profile()
        {
            var viewModel = GetWallViewModel();

            return View("UserProfile", viewModel);
        }

        [Route("Dashboard/admin/profile/edit")]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit()
        {
            var viewModel = GetWallViewModel();

            return View("UserProfileEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Dashboard/admin/profile/save")]
        [Authorize(Roles = "Admin")]
        public ActionResult Save(WallViewModel ViewModel)
        {
            if (ViewModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var person = ViewModel.User;

            // Get logged in admin
            var loggedInAdmin = AccountController.GetUserFromEmail(User.Identity.Name);

            if (loggedInAdmin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid)
            {
                var viewModel = new WallViewModel
                {
                    User = loggedInAdmin
                };
                return View("UserProfileEdit", viewModel);
            }

            //* Edit admin data and save it
            loggedInAdmin.FirstName = person.FirstName;
            loggedInAdmin.LastName = person.LastName;
            loggedInAdmin.Password = person.Password; // Need hash later
            loggedInAdmin.PhoneNumber = person.PhoneNumber;
            //LoggedInAdmin.Photo = person.Photo; // Need change later

            Db.SaveChanges();

            return RedirectToAction("Profile", "Dashboard");
        }

        // Get: Dashboard/Posts/Pending
        [Route("Dashboard/posts/pending")]
        [Authorize(Roles = "Admin")]
        public ActionResult PendingPosts()
        {
            var PendingPosts = GetAllPosts().Where(y => y.Status == Post.PENDING);

            return View(PendingPosts);
        }

        // Get: Dashboard/Posts
        [Route("Dashboard/posts")]
        [Authorize(Roles = "Admin")]
        public ActionResult AllPosts()
        {
            var allPosts = GetAllPosts();

            return View(allPosts);
        }

        [Route("Dashboard/posts/delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //* Delete Post
            var post = Db.Posts.Single(p => p.Id == id);
            Db.Posts.Attach(post);
            Db.Posts.Remove(post);
            Db.SaveChanges();

            return Json(new { success = true, message = "Post deleted successfully!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
            var posts = Db.Posts.Include("Agency").ToList();
            return posts;
        }

        public WallViewModel GetWallViewModel()
        {
            // Get logged in admin ID
            var loggedInAdmin = AccountController.GetUserFromEmail(User.Identity.Name);

            var viewModel = new WallViewModel
            {
                User = loggedInAdmin
            };

            return viewModel;
        }
    }
}