using System.Collections.Generic;
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
        public ActionResult Index()
        {
            var posts = Db.Posts.Include("Agency").Where(p => p.Status.Equals(Post.APPROVED)).ToList();

            var viewModel = new WallViewModel
            {
                Posts =  posts
            };

            return View(viewModel);
        }

        // Get: Wall/posts/saved
        [Route("Wall/posts/saved")]
        public ActionResult SavedPosts()
        {
            var posts = Db.Posts.Include("Agency").Where(p => p.Status.Equals(Post.APPROVED)).ToList();

            var viewModel = new WallViewModel
            {
                Posts = posts
            };

            return View(viewModel);
        }
    }
}