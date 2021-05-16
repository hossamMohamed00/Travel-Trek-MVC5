using System.Collections.Generic;
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

        [Route("Dashboard/admin/profile")]
        public ActionResult Profile()
        {
            var admin = Db.Users.SingleOrDefault(u => u.Id == 1); // Need Edit later
            return View(admin);
        }

        /* Helper Methods */

        // Return all users (agencies and travelers)
        public IEnumerable<Person> GetAllUsers()
        {
            var User = Db.Users.ToList();
            return User;
        }

        // Return all users (agencies and travelers)
        public IEnumerable<Post> GetAllPosts()
        {
            var posts = Db.Posts.ToList();
            return posts;
        }
    }
}