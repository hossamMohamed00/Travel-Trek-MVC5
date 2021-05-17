using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Travel_Trek.Db_Context;
using Travel_Trek.Models;

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
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var user = Db.Users.Single(c =>c.Id==id);
            Db.Users.Remove(user);
            Db.SaveChanges();
            return RedirectToAction("AllUsers");
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

        // Get: Dashboard/Profile
        [Route("Dashboard/Admin/Profile")]
        public ActionResult Profile()
        {
            var admin = Db.Users.SingleOrDefault(u => u.Id == 1); // Need Edit later
            return View(admin);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Person person)
        {
            if (!ModelState.IsValid)
            {
                var admin = Db.Users.SingleOrDefault(u => u.Id == 1); // Need Edit later
                return View("Profile", admin);
            }

            var adminInDb = Db.Users.Single(m => m.Id == person.Id);
            adminInDb.FirstName = person.FirstName;
            adminInDb.LastName = person.LastName;
            adminInDb.Password = person.Password; // Need hash later
            adminInDb.PhoneNumber = person.PhoneNumber;
            //adminInDb.Photo = person.Photo; // Need change later

            Db.SaveChanges();
            return RedirectToAction("Index", "Dashboard");

        }


        public ActionResult DeletePost(int id)
        {
            //* Delete Post
            var post = Db.Posts.Single(p => p.Id == id);
            Db.Posts.Attach(post);
            Db.Posts.Remove(post);
            Db.SaveChanges();

            return RedirectToAction("AllPosts");

        }

        public ActionResult ApprovePost(int id)
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
            return RedirectToAction("PendingPosts");

        }

        public ActionResult RefusePost(int id)
        {
            var post = Db.Posts.Single(p => p.Id == id);
            post.Status = Post.REFUSED;
            Db.SaveChanges();

            return RedirectToAction("PendingPosts");

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
    }
}