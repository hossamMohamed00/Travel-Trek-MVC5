using System.Linq;
using System.Web.Mvc;
using Travel_Trek.Db_Context;
using Travel_Trek.Models;
using Travel_Trek.ViewModels;

namespace Travel_Trek.Controllers
{
    public class WallController : Controller
    {
        readonly ApplicationDbContext Db;

        /* Constructor */
        public WallController()
        {
            Db = new ApplicationDbContext();
        }

        /* Override Dispose method */
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
        }

        // GET: Wall
        // [Authorize(Roles = "Traveler")]
        public ActionResult Index()
        {
            var posts = Db.Posts.Include("Agency").Where(p => p.Status.Equals(Post.APPROVED)).ToList();

            var viewModel = new WallViewModel
            {
                Posts = posts,
                Login = new Login()
            };

            return View(viewModel);
        }

        // Get: Wall/posts/saved
        [Route("Wall/posts/saved")]
        [Authorize(Roles = "Traveler")]
        public ActionResult SavedPosts()
        {
            var posts = Db.Posts.Include("Agency").Where(p => p.Status.Equals(Post.APPROVED)).ToList();

            var viewModel = new WallViewModel
            {
                Posts = posts
            };

            return View(viewModel);
        }


        [Route("Wall/user/profile")]
        [Authorize(Roles = "Traveler")]
        public ActionResult Profile()
        {
            var viewModel = GetWallViewModel();


            return View("UserProfile", viewModel);
        }

        [Route("Wall/user/profile/edit")]
        [Authorize(Roles = "Traveler")]
        public ActionResult Edit()
        {
            var viewModel = GetWallViewModel();

            return View("UserProfileEdit", viewModel);
        }

        /* Helper Methods */
        public WallViewModel GetWallViewModel()
        {
            var travler = Db.Users.Include("UserRole").SingleOrDefault(u => u.Id == 3); // Need Edit later

            var viewModel = new WallViewModel
            {
                User = travler
            };

            return viewModel;
        }
    }
}