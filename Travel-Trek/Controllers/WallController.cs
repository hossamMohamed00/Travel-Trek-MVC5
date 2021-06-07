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
                travelerInDb.Photo = ImagePath;
            }

            //* Save the changes in the database
            _dbContext.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Update data successfully 🐱‍🏍✌"
            },
                JsonRequestBehavior.AllowGet);
        }

        // Get: Wall/posts/saved
        [Route("Wall/posts/saved")]
        [Authorize(Roles = RoleNamesAndIds.Traveler)]
        public ActionResult SavedPosts()
        {
            var viewModel = GetWallViewModelForSavedPosts();

            return View(viewModel);
        }

        [Route("Wall/posts/questions")]
        [Authorize(Roles = RoleNamesAndIds.Traveler)]
        public ActionResult UserQuestions()
        {
            var viewModel = GetWallViewModelForUserQuestions();

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
        [Route("Wall/posts/like")] // Referenced in PostOperations.js
        public ActionResult LikePost(int postId)
        {
            // Get Logged in agency
            var travelerId = AccountController.GetUserFromEmail(User.Identity.Name).Id;

            // Get the post from the db
            var post = _dbContext.Posts.Single(p => p.Id == postId);

            //* Check first if the post is already liked or not
            var isLiked = IsAlreadyLiked(travelerId, postId);
            if (isLiked)
            {
                /* Dislike the post here */

                //* Get this liked post
                var likedPost = _dbContext.LikedPosts.SingleOrDefault(p => p.UserId == travelerId && p.PostId == postId);

                if (likedPost == null)
                {
                    return Json(new { success = false, message = "Cannot like this post, please try again later on 😪🔃" }, JsonRequestBehavior.AllowGet);
                }

                //* remove the post from the liked posts for this user
                _dbContext.LikedPosts.Remove(likedPost);

                //* Decrease post likes
                post.Likes -= 1;

                // Save changes
                _dbContext.SaveChanges();

                return Json(new { success = true, likes = post.Likes, message = "Trip Post disliked Successfully! 💔" }, JsonRequestBehavior.AllowGet);
            }

            // If the post doesn't liked by this user before, So like it....

            //* Add the post to the liked posts for this user
            _dbContext.LikedPosts.Add(new LikedPosts { PostId = postId, UserId = travelerId });

            //* Increase post likes
            post.Likes += 1;

            // Save changes
            _dbContext.SaveChanges();

            //* Send json to the user
            return Json(new { success = true, likes = post.Likes, message = "Trip Post Liked Successfully! ❤" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = RoleNamesAndIds.Traveler)]
        [Route("Wall/posts/dislike")] // Referenced in PostOperations.js
        public ActionResult DisLikePost(int postId)
        {
            // Get Logged in agency
            var travelerId = AccountController.GetUserFromEmail(User.Identity.Name).Id;

            // Get the post from the db
            var post = _dbContext.Posts.Single(p => p.Id == postId);

            //* Check first if the post is already disliked or not
            var isAlreadyDisLiked = IsAlreadyDisLiked(travelerId, postId);
            if (isAlreadyDisLiked)
            {
                /* Remove Dislike here */

                //* Get this disliked post
                var disLikedPost = _dbContext.DisLikedPosts.SingleOrDefault(p => p.UserId == travelerId && p.PostId == postId);

                if (disLikedPost == null)
                {
                    return Json(new { success = false, message = "Cannot dislike this post, please try again later on 😪🔃" }, JsonRequestBehavior.AllowGet);
                }

                //* remove the post from the disliked posts for this user
                _dbContext.DisLikedPosts.Remove(disLikedPost);

                //* Decrease post likes
                post.DisLikes -= 1;

                // Save changes
                _dbContext.SaveChanges();

                return Json(new { success = true, dislikes = post.DisLikes, message = "Remove dislike from this Trip Post done Successfully! 💔" }, JsonRequestBehavior.AllowGet);
            }

            // If the post doesn't disliked by this user before, So dislike it....

            //* Add the post to the disliked posts for this user
            _dbContext.DisLikedPosts.Add(new DisLikedPosts { PostId = postId, UserId = travelerId });

            //* Increase post dislikes
            post.DisLikes += 1;

            // Save changes
            _dbContext.SaveChanges();

            //* Send json to the user
            return Json(new { success = true, dislikes = post.DisLikes, message = "Trip Post disliked Successfully! 💔👎🏻" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = RoleNamesAndIds.Traveler)]
        [Route("Wall/posts/ask")] // Referenced in PostOperations.js
        public ActionResult AskQuestion(int postId, string question)
        {
            // Get Logged in agency
            var travelerId = AccountController.GetUserFromEmail(User.Identity.Name).Id;

            // Get the post from the db
            var post = _dbContext.Posts.Single(p => p.Id == postId);

            //* Check first if this user asked a question on this post before
            var askedBefore = IsUserAlreadyAsked(travelerId, postId);
            if (askedBefore)
            {
                /* Tell the user that he is already ask a question on this question and give him the reply*/

                //* Get this liked post
                var userQuestion = _dbContext.UserQuestions.SingleOrDefault(p => p.UserId == travelerId && p.PostId == postId);

                if (userQuestion == null)
                {
                    return Json(new { success = false, message = "Cannot process this operation, please try again later on 😪🔃" }, JsonRequestBehavior.AllowGet);
                }

                var messageText = "You already have a question on this post was at " + userQuestion.Date.ToLongDateString() +
                                  ". Go ahead to Your questions and see the reply ✌🤗.";
                return Json(new { success = false, message = messageText }, JsonRequestBehavior.AllowGet);
            }

            // If the post doesn't haveany question by this user before, So save his question ....

            //* Add the post to the User Questions 
            _dbContext.UserQuestions.Add(new UserQuestion { PostId = postId, UserId = travelerId, Question = question, Status = UserQuestion.Open });

            // Save changes
            _dbContext.SaveChanges();

            //* Send json to the user
            return Json(new { success = true, message = "Yor question sent successfully to the agency, when they answers you, we will notify you ASAP ❤🐱‍🏍" }, JsonRequestBehavior.AllowGet);
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

        public WallViewModel GetWallViewModelForUserQuestions()
        {
            // Get Logged in agency
            var loggedInTraveler = AccountController.GetUserFromEmail(User.Identity.Name);

            //* Get the user saved posts
            var userQuestions = _dbContext
                .Users
                .Where(u => u.Id == loggedInTraveler.Id)
                .SelectMany(p => p.UserQuestions)
                .Include(p => p.Post.Agency)
                .Include(p => p.User)
                .ToList();

            var viewModel = new WallViewModel
            {
                UserQuestions = userQuestions,
                CurrentView = "UserQuestions"
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

        public bool IsAlreadyLiked(int userId, int postId)
        {
            //* Get this liked post
            var likedPost = _dbContext.LikedPosts.
                SingleOrDefault(p => p.UserId == userId && p.PostId == postId);

            //* Return true if there are a post
            return likedPost != null ? true : false;
        }

        public bool IsAlreadyDisLiked(int userId, int postId)
        {
            //* Get this disliked post
            var dislikedPost = _dbContext.DisLikedPosts.
                SingleOrDefault(p => p.UserId == userId && p.PostId == postId);

            //* Return true if there are a post
            return dislikedPost != null ? true : false;
        }

        public bool IsUserAlreadyAsked(int userId, int postId)
        {
            //* Get this question if exists
            var userQuestion = _dbContext.UserQuestions.
                SingleOrDefault(p => p.UserId == userId && p.PostId == postId);

            //* Return true if there are a post
            return userQuestion != null ? true : false;
        }
    }
}