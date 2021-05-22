using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel_Trek.Db_Context;
using Travel_Trek.Helpers;
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
                Login = new Login(),
                CurrentView = "Index"
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
                Posts = posts,
                CurrentView = "Index"
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Traveler")]
        public ActionResult Save(WallViewModel viewModel, HttpPostedFileBase userPhoto)
        {
            // Check if the model in valid state or not
            if (!ModelState.IsValid)
            {
                var wallViewModel = GetWallViewModel();

                return View("UserProfileEdit", wallViewModel);
            }

            //* Get the user data from the ViewModel
            var person = viewModel.User;

            //* Get traveler from the database
            var travelerInDb = Db.Users.Include("UserRole").Single(m => m.Id == person.Id);

            //* Edit traveler data and save it
            travelerInDb.FirstName = person.FirstName;
            travelerInDb.LastName = person.LastName;
            travelerInDb.Password = person.Password; // Need hash later
            travelerInDb.PhoneNumber = person.PhoneNumber;

            //* Check if the traveler provide a photo
            if (userPhoto != null)
            {
                var ImagePath = Utilities.GetPersonImagePath(userPhoto);

                // Save the image on the device 
                userPhoto.SaveAs(Server.MapPath(ImagePath));
                travelerInDb.Photo = ImagePath;
            }

            //* Save the changes in the database
            Db.SaveChanges();

            return RedirectToAction("Profile", "Wall");
        }


        [HttpPost]
        [Authorize(Roles = "Traveler")]
        public ActionResult LikePost(int Id)
        {
            //Get post from db
            var post = Db.Posts.Single(p => p.Id == Id);

            if (post == null)
                return Json(new { success = false, message = "Cannot find this post!" }, JsonRequestBehavior.AllowGet);

            // Increment post likes
            post.Likes += 1;

            // Save changes
            Db.SaveChanges();

            //* Send json to the user
            return Json(new { success = true, likes = post.Likes }, JsonRequestBehavior.AllowGet);

        }

        /* Helper Methods */
        public WallViewModel GetWallViewModel()
        {
            // Get Logged in agency
            var loggedInTravler = AccountController.GetUserFromEmail(User.Identity.Name);

            var travler = Db.Users.Include("UserRole").SingleOrDefault(u => u.Id == loggedInTravler.Id);

            var viewModel = new WallViewModel
            {
                User = travler,
                CurrentView = ""
            };

            return viewModel;
        }
    }
}