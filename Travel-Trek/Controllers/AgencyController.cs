using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel_Trek.Db_Context;
using Travel_Trek.Helpers;
using Travel_Trek.Models;
using Travel_Trek.ViewModels;

namespace Travel_Trek.Controllers
{
    public class AgencyController : Controller
    {
        ApplicationDbContext Db;

        /* Constructor */
        public AgencyController()
        {
            Db = new ApplicationDbContext();
        }

        /* Override Dispose method */
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
        }

        // GET: Agency
        [Authorize(Roles = "Agency")]
        public ActionResult Index()
        {
            return View();
        }

        // Get: Agency/Profile
        [Route("Agency/Profile")]
        [Authorize(Roles = "Agency")]
        public ActionResult Profile()
        {
            var viewModel = GetWallViewModel();

            return View("UserProfile", viewModel);
        }

        [Route("Agency/Profile/Edit")]
        [Authorize(Roles = "Agency")]
        public ActionResult Edit()
        {
            var viewModel = GetWallViewModel();

            return View("UserProfileEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agency")]
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

            //* Get agency from the database
            var agencyInDb = Db.Users.Include("UserRole").Single(m => m.Id == person.Id);

            //* Edit agency data and save it
            agencyInDb.FirstName = person.FirstName;
            agencyInDb.LastName = person.LastName;
            agencyInDb.Password = person.Password; // Need hash later
            agencyInDb.PhoneNumber = person.PhoneNumber;

            //* Check if the agency provide a photo
            if (userPhoto != null)
            {
                var ImagePath = Utilities.GetPersonImagePath(userPhoto);

                // Save the image on the device 
                userPhoto.SaveAs(Server.MapPath(ImagePath));
                agencyInDb.Photo = ImagePath;
            }

            //* Save the changes in the database
            Db.SaveChanges();

            return RedirectToAction("Profile", "Agency");
        }

        [Route("Agency/Posts/Create")]
        [Authorize(Roles = "Agency")]
        public ActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Agency")]

        public JsonResult PublishPost(Post post, HttpPostedFileBase TripImage)
        {
            if (ModelState.IsValid)
            {
                // Get the agency which wants to add this post
                var agency = AccountController.GetUserFromEmail(User.Identity.Name);

                var ImagePath = Utilities.GetPostImagePath(TripImage, post.PostDate);

                // Save the image on the device 
                TripImage.SaveAs(Server.MapPath(ImagePath));

                //* Save image data
                post.TripImage = ImagePath;

                post.AgencyId = agency.Id;

                Db.Posts.Add(post);
                Db.SaveChanges();

                return Json(new { success = true, message = "Trip Post request sent successfully, our admins will review the post ASAP 🐱‍🏍💕" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Cannot publish this Trip Post right now 😒😭" }, JsonRequestBehavior.AllowGet);

        }

        [Authorize(Roles = "Agency")]
        [Route("Agency/Posts")]
        [Authorize(Roles = "Agency")]
        public ActionResult MyPosts()
        {
            // Get Logged in agency
            var agency = AccountController.GetUserFromEmail(User.Identity.Name);

            // Get posts for this agency
            var posts = Db.Posts.Include("Agency").Where(p => p.AgencyId == agency.Id).ToList();

            return View(posts);
        }

        [Authorize(Roles = "Agency")]
        [Route("Agency/Posts/delete")]
        public ActionResult DeletePost(int? id)
        {
            //* Check if the id not provided
            if (id == null)
                return Json(new { success = false, message = "Cannot delete this post right now! 😭" }, JsonRequestBehavior.AllowGet);

            //* Delete the post
            var isDeleted = Utilities.DeletePostFromDb(id, Db);

            //* Inform the user 
            return (isDeleted)
                ? Json(new { success = true, message = "Post deleted successfully! 🦾✌" },
                    JsonRequestBehavior.AllowGet)
                : Json(new { success = false, message = "Cannot delete this post right now! 😭" },
                    JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "Agency")]
        [Route("Agency/FAQ")]
        public ActionResult FAQ()
        {
            return View();
        }

        /* Helper Methods */
        public WallViewModel GetWallViewModel()
        {
            // Get Logged in agency
            var agency = AccountController.GetUserFromEmail(User.Identity.Name);

            var viewModel = new WallViewModel
            {
                User = agency
            };

            return viewModel;
        }
    }
}