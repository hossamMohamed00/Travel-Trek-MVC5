using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Travel_Trek.Db_Context;
using Travel_Trek.Models;

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
        [Authorize(Roles = "Traveler")]
        public ActionResult Index()
        {
            var posts = GetAllPosts();

            return View(posts);

        }

        // Get: Wall/posts/saved
        [Route("Wall/posts/saved")]
        [Authorize(Roles = "Traveler")]
        public ActionResult SavedPosts()
        {
            var posts = GetAllPosts();

            return View(posts);
        }

        /* Helper methods */

        // Return all users (agencies and travelers)
        public IEnumerable<Post> GetAllPosts()
        {
            //* Filter the posts to get the approved only
            var posts = Db.Posts.Include("Agency").Where(p => p.Status.Equals(Post.APPROVED));
            return posts;
        }


    }
}