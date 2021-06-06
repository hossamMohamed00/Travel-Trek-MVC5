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
    [Authorize(Roles = RoleNamesAndIds.Agency)]
    public class AgencyController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        /* Constructor */
        public AgencyController()
        {
            _dbContext = new ApplicationDbContext();
        }

        /* Override Dispose method */
        protected override void Dispose(bool disposing)
        {
            _dbContext.Dispose();
        }

        // Get: Agency/Profile
        [Route("Agency/Profile")]
        public ActionResult Profile()
        {
            var viewModel = GetWallViewModel();

            return View("UserProfile", viewModel);
        }

        [Route("Agency/Profile/Edit")]
        public ActionResult Edit()
        {
            var viewModel = GetWallViewModel();

            return View("UserProfileEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
            var agencyInDb = _dbContext.Users.Include(u => u.UserRole).Single(m => m.Id == person.Id);

            //* Edit agency data and save it
            agencyInDb.FirstName = person.FirstName;
            agencyInDb.LastName = person.LastName;
            agencyInDb.Password = person.Password; // Need hash later
            agencyInDb.PhoneNumber = person.PhoneNumber;

            //* Check if the agency provide a photo
            if (userPhoto != null)
            {
                //* Validate the extension
                var isValid = Utilities.ValidateImageExtension(userPhoto);

                if (!isValid)
                    return Json(new
                    {
                        success = false,
                        message = "You must upload a post image with one of the allowed image extensions [PNG / JPG / JPEG] 😡"
                    },
                        JsonRequestBehavior.AllowGet);

                //* if the image is valid, ....
                var ImagePath = Utilities.GetPersonImagePath(userPhoto);

                // Save the image on the device 
                userPhoto.SaveAs(Server.MapPath(ImagePath));
                agencyInDb.Photo = ImagePath;
            }

            //* Save the changes in the database
            _dbContext.SaveChanges();

            return RedirectToAction("Profile", "Agency");
        }

        [Route("Agency/Posts/Create")]
        public ActionResult CreatePost()
        {
            var viewModel = new PostFormViewModel
            {
                Post = new Post(),
                Action = "PublishPost",
                Header = "Create New Trip Post 😉🛶",
                Operation = "Create",
                Title = "Publish New Trip Post"
            };

            return View("PostForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PublishPost(PostFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //* Get the post
                var post = viewModel.Post;

                //* Get the image from the viewModel
                var tripImage = viewModel.TripImage;

                //* Validate the extension
                var isValid = Utilities.ValidateImageExtension(tripImage);

                if (!isValid)
                    return Json(new
                    {
                        success = false,
                        message = "You must upload a post image with one of the allowed image extensions [PNG / JPG / JPEG] 😡"
                    },
                        JsonRequestBehavior.AllowGet);

                //* if the image is valid, ....

                var ImagePath = Utilities.GetPostImagePath(tripImage, post.PostDate);

                // Save the image on the device 
                tripImage.SaveAs(Server.MapPath(ImagePath));

                //* Save image data
                post.TripImage = ImagePath;

                // Get the agency which wants to add this post
                var agency = AccountController.GetUserFromEmail(User.Identity.Name);

                //* Set the agency which created the post
                post.AgencyId = agency.Id;

                //* Add the post to the db and save the changes
                _dbContext.Posts.Add(post);
                _dbContext.SaveChanges();

                return Json(new { success = true, message = "Trip Post request sent successfully, our admins will review the post ASAP 🐱‍🏍💕" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Cannot publish this Trip Post right now 😒😭" }, JsonRequestBehavior.AllowGet);
        }

        [Route("Agency/Posts/update")]
        public ActionResult UpdatePost(int id)
        {
            var viewModel = GetUpdatePostFormViewModel(id);

            return View("PostForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePostData(PostFormViewModel ViewModel)
        {
            // Check if the model in valid state or not
            if (!ModelState.IsValid)
            {
                // var viewModel = GetUpdatePostFormViewModel(ViewModel.Post.Id);

                return Json(new { success = false, message = "Something went wrong. Please try again 🔃" });
            }

            //* Get the Post data from the ViewModel
            var post = ViewModel.Post;

            //* Get the image from the viewModel
            var tripImage = ViewModel.TripImage;

            //* Get post from the database
            var postInDb = _dbContext.Posts.Single(p => p.Id == post.Id);

            //* Edit post data and save it
            postInDb.TripTitle = post.TripTitle;
            postInDb.TripDestination = post.TripDestination;
            postInDb.Price = post.Price;
            postInDb.TripDetails = post.TripDetails;
            postInDb.TripDate = post.TripDate;
            postInDb.Status = Post.PENDING; // Change the status back to be pending

            //* Check if there is a photo for the trip
            if (tripImage != null)
            {
                //* Validate the extension
                var isValid = Utilities.ValidateImageExtension(tripImage);

                if (!isValid)
                    return Json(new
                    {
                        success = false,
                        message = "You must upload a post image with one of the allowed image extensions [PNG / JPG / JPEG] 😡"
                    },
                        JsonRequestBehavior.AllowGet);

                //* if the image is valid, ....

                //* Delete the previous image of the post if he already has one
                if (!string.IsNullOrEmpty(postInDb.TripImage))
                {
                    Utilities.DeleteImageFromServer(postInDb.TripImage);
                }

                //* Get the path of the new photo
                var ImagePath = Utilities.GetPostImagePath(tripImage, postInDb.PostDate);

                // Save the image on the device 
                tripImage.SaveAs(Server.MapPath(ImagePath));

                //* Set the path to the post's photo field
                postInDb.TripImage = ImagePath;
            }

            //* Save the changes in the database
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Trip Post Update Successfully ✔🕺🏻, Our admins will review the post ASAP 🐱‍🏍🔃" }, JsonRequestBehavior.AllowGet);
        }

        [Route("Agency/")]
        public ActionResult MyPosts()
        {
            // Get Logged in agency
            var agency = AccountController.GetUserFromEmail(User.Identity.Name);

            // Get posts for this agency
            var posts = _dbContext.Posts.Include(p => p.Agency).Where(p => p.AgencyId == agency.Id).ToList();

            return View(posts);
        }

        [Route("Agency/Posts/delete")] // Reference in AgencyDeletePost.js
        public ActionResult DeletePost(int? id)
        {
            //* Check if the id not provided
            if (id == null)
                return Json(new { success = false, message = "Cannot delete this post right now! 😭" }, JsonRequestBehavior.AllowGet);

            //* Delete the post
            var isDeleted = Utilities.DeletePostFromDb(id, _dbContext);

            //* Inform the user 
            return (isDeleted)
                ? Json(new { success = true, message = "Post deleted successfully! 🦾✌" },
                    JsonRequestBehavior.AllowGet)
                : Json(new { success = false, message = "Cannot delete this post right now! 😭" },
                    JsonRequestBehavior.AllowGet);
        }


        [Route("Agency/FAQ")]
        public ActionResult FAQ()
        {
            // Get Logged in agency
            var agency = AccountController.GetUserFromEmail(User.Identity.Name);

            //* Get all the questions for this agency
            var questions = _dbContext.UserQuestions.Include(q => q.Post.Agency).Include(q => q.User).Where(q => q.Post.AgencyId == agency.Id).OrderByDescending(q => q.Date).ToList();

            return View(questions);
        }

        [HttpPost]
        [Route("Agency/FAQ/reply")]
        public JsonResult ReplyToQuestion(int? postId, int? userId, string reply)
        {
            if (postId == null || userId == null || reply.Length == 0)
            {
                return Json(
                    new { success = false, message = "Error while reply to this question, please try again 😐🔃" },
                    JsonRequestBehavior.AllowGet);
            }

            //* Get the post to add the reply to it
            var question = _dbContext.UserQuestions.Single(q => q.PostId == postId && q.UserId == userId);

            //* Add the reply
            question.Answer = reply;

            //* Update the status of the question
            question.Status = UserQuestion.Closed;

            //* Save all the changes to the database
            _dbContext.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Great Job ❤,Your reply to this question sent successfully to the traveler 🐱‍🏍✔ "
            }, JsonRequestBehavior.AllowGet);
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

        public PostFormViewModel GetUpdatePostFormViewModel(int id)
        {
            // Get Logged in agency
            var agency = AccountController.GetUserFromEmail(User.Identity.Name);
            var viewModel = new PostFormViewModel
            {
                Post = _dbContext.Posts.Include(p => p.Agency).Single(p => p.Id == id && p.AgencyId == agency.Id),
                Action = "UpdatePostData",
                Header = "Update Trip Post 📑🧐",
                Operation = "Update",
                Title = "Update Trip Post 📑🧐"
            };

            return viewModel;
        }

    }
}