using System.Data.Entity;
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
        private readonly ApplicationDbContext _dbContext;

        /* Constructor */
        public WallController()
        {
            _dbContext = new ApplicationDbContext();
        }

        /* Override Dispose method */
        protected override void Dispose(bool disposing)
        {
            _dbContext.Dispose();
        }

        // GET: Wall
        // [Authorize(Roles = "Traveler")]
        public ActionResult Index()
        {
            var posts = _dbContext.Posts.Include(p => p.Agency).Where(p => p.Status.Equals(Post.APPROVED)).ToList();

            var viewModel = new WallViewModel
            {
                Posts = posts,
                Login = new Login(),
                CurrentView = "Index"
            };

            return View(viewModel);
        }

        [Route("Wall/user/profile")]
        [Authorize(Roles = RoleNamesAndIds.Traveler)]
        public ActionResult Profile()
        {
            var viewModel = GetWallViewModel();

            return View("UserProfile", viewModel);
        }

        [Route("Wall/user/profile/edit")]
        [Authorize(Roles = RoleNamesAndIds.Traveler)]
        public ActionResult Edit()
        {
            var viewModel = GetWallViewModel();

            return View("UserProfileEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNamesAndIds.Traveler)]
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
            var travelerInDb = _dbContext.Users.Include(u => u.UserRole).Single(m => m.Id == person.Id);

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
            _dbContext.SaveChanges();

            return RedirectToAction("Profile", "Wall");
        }

        // Get: Wall/posts/saved
        [Route("Wall/posts/saved")]
        [Authorize(Roles = RoleNamesAndIds.Traveler)]
        public ActionResult SavedPosts()
        {
            var viewModel = GetWallViewModelForSavedPosts();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = RoleNamesAndIds.Traveler)]
        [Route("Wall/posts/save")] // Referenced in PostOperations.js
        public ActionResult SavePost(int postId)
        {
            // Get Logged in agency
            var travelerId = AccountController.GetUserFromEmail(User.Identity.Name).Id;

            //* Check first if the post is already saved or not
            var isSaved = IsAlreadySaved(travelerId, postId);

            if (isSaved)
            {
                return Json(new { success = false, message = "This Trip Post already saved in your saved posts, Go to Saved Posts tab to see it 💾😁" }, JsonRequestBehavior.AllowGet);
            }

            //* Add the post to the saved posts for this user
            _dbContext.SavedPosts.Add(new SavedPosts { PostId = postId, UserId = travelerId });

            if (false)
            {
                return Json(new { success = false, message = "Cannot save this post, please try again later on 😪🔃" }, JsonRequestBehavior.AllowGet);
            }

            // Save changes
            _dbContext.SaveChanges();

            //* Send json to the user
            return Json(new { success = true, message = "Trip Post Saved Successfully!🤗💾 , Navigate to Saved Posts tab to see it ✌" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = RoleNamesAndIds.Traveler)]
        [Route("Wall/posts/removeSaved")] // Referenced in RemoveSavedPost.js
        public ActionResult UnSavePost(int postId)
        {
            // Get Logged in agency
            var travelerId = AccountController.GetUserFromEmail(User.Identity.Name).Id;

            //* Get this saved post
            var savedPost = _dbContext.SavedPosts.SingleOrDefault(p => p.UserId == travelerId && p.PostId == postId);

            if (savedPost == null)
            {
                return Json(new { success = false, message = "Cannot save this post, please try again later on 😪🔃" }, JsonRequestBehavior.AllowGet);
            }

            //* remove the post from the saved posts for this user
            _dbContext.SavedPosts.Remove(savedPost);

            // Save changes
            _dbContext.SaveChanges();

            //* Send json to the user
            return Json(new { success = true, message = "Trip Post removed from your saved posts 🤷‍♂️😐" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = RoleNamesAndIds.Traveler)]
        public ActionResult LikePost(int id)
        {
            //Get post from db
            var post = _dbContext.Posts.Single(p => p.Id == id);

            if (post == null)
                return Json(new { success = false, message = "Cannot find this post!" }, JsonRequestBehavior.AllowGet);

            // Increment post likes
            post.Likes += 1;

            // Save changes
            _dbContext.SaveChanges();

            //* Send json to the user
            return Json(new { success = true, likes = post.Likes }, JsonRequestBehavior.AllowGet);

        }

        /* Helper Methods */
        public WallViewModel GetWallViewModel()
        {
            // Get Logged in agency
            var loggedInTravler = AccountController.GetUserFromEmail(User.Identity.Name);

            var travler = _dbContext.Users.Include(u => u.UserRole).SingleOrDefault(u => u.Id == loggedInTravler.Id);

            var viewModel = new WallViewModel
            {
                User = travler,
                CurrentView = ""
            };

            return viewModel;
        }

        public WallViewModel GetWallViewModelForSavedPosts()
        {
            // Get Logged in agency
            var loggedInTraveler = AccountController.GetUserFromEmail(User.Identity.Name);

            //* Get the user saved posts
            var savedPosts = _dbContext
                .Users
                .Where(u => u.Id == loggedInTraveler.Id)
                .SelectMany(p => p.SavedPosts)
                .Include(p => p.Post.Agency)
                .ToList();

            var viewModel = new WallViewModel
            {
                SavedPosts = savedPosts,
                CurrentView = "SavedPosts"
            };
            return viewModel;
        }

        public bool IsAlreadySaved(int userId, int postId)
        {
            //* Get this saved post
            var savedPost = _dbContext.SavedPosts.
                SingleOrDefault(p => p.UserId == userId && p.PostId == postId);

            //* Return true if there are a post
            return savedPost != null ? true : false;
        }
    }
}