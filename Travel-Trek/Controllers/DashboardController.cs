using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel_Trek.Db_Context;
using Travel_Trek.Helpers;
using Travel_Trek.Models;
using Travel_Trek.ViewModels;

namespace Travel_Trek.Controllers
{
    [Authorize(Roles = RoleNamesAndIds.Admin)]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        /* Constructor */
        public DashboardController()
        {
            /* Initalize the db contextObject */
            _dbContext = new ApplicationDbContext();
        }

        /* Override Dispose method */
        protected override void Dispose(bool disposing)
        {
            _dbContext.Dispose();
        }

        /*---------------------------------------------------*/

        /* Dashboard (Admin) Actions */

        // GET: Dashboard
        [Route("Dashboard/")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Dashboard/Users
        [Route("Dashboard/users")]
        public ActionResult AllUsers()
        {
            var users = GetAllUsers();

            return View(users);
        }

        [Route("Dashboard/users/new")]
        public ActionResult CreateUser()
        {
            var viewModel = new AddUserViewModel();

            return View(viewModel);
        }

        //* Create User Logic Will be here
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("Dashboard/users/new/save")]
        public ActionResult CreateNewUser(AddUserViewModel viewModel, HttpPostedFileBase userPhoto)
        {
            //* Check if the model state is valid
            if (!ModelState.IsValid)
            {
                var addUserViewModel = new AddUserViewModel();

                return View("CreateUser", addUserViewModel);
            }

            //* Get the User data from the ViewModel
            var user = viewModel.Person;

            //* Check if the given email is a unique or not
            var ExistsUser = _dbContext.Users.SingleOrDefault(u => u.Email == user.Email);

            var isTakenBefore = ExistsUser != null;

            //* If true, so the email already exists
            if (isTakenBefore)
            {
                return Json(new { success = false, message = "Email already taken, choose another one 😋" }, JsonRequestBehavior.AllowGet);
            }

            //* Check if the admin provide a photo
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
                user.Photo = ImagePath;
            }

            //Save the user
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return Json(new
            {
                success = true,
                message = "New user added successfully, Go to all users page to see it now 🙈🤗"
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("Dashboard/users/delete")]// Reference in AdminDeleteUser.js
        public ActionResult DeleteUser(int? id)
        {
            if (id == null)
                return Json(new { success = true, message = "Cannot delete this user right now! 😭" }, JsonRequestBehavior.AllowGet);

            //* Get the user from the db
            var user = _dbContext.Users.Single(c => c.Id == id);

            //* Remove user image from the device
            if (!String.IsNullOrEmpty(user.Photo))
            {
                Utilities.DeleteImageFromServer(user.Photo);
            }

            //* Remove the liked and saved post by this user
            var savedPosts = _dbContext.SavedPosts.Where(p => p.UserId == id);
            _dbContext.SavedPosts.RemoveRange(savedPosts);

            var postLikes = _dbContext.LikedPosts.Where(p => p.UserId == id);
            _dbContext.LikedPosts.RemoveRange(postLikes);


            //* Remove the user from the db
            _dbContext.Users.Remove(user);

            //* Save the changes to the db
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "User deleted successfully" }, JsonRequestBehavior.AllowGet);
        }

        // Get: User/Profile
        [Route("Dashboard/admin/profile")]
        public ActionResult Profile()
        {
            var viewModel = GetWallViewModel();

            return View("UserProfile", viewModel);
        }

        [Route("Dashboard/admin/profile/edit")]
        public ActionResult Edit()
        {
            var viewModel = GetWallViewModel();

            return View("UserProfileEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Dashboard/admin/profile/save")]
        public ActionResult Save(WallViewModel ViewModel, HttpPostedFileBase userPhoto)
        {
            // Check if the model in valid state or not
            if (!ModelState.IsValid)
            {
                var viewModel = GetWallViewModel();
                return View("UserProfileEdit", viewModel);
            }

            //* Get the User data from the ViewModel
            var person = ViewModel.User;

            //* Get admin from the database
            var adminInDb = _dbContext.Users.Include(u => u.UserRole).Single(u => u.Id == person.Id);

            //* Edit admin data and save it
            adminInDb.FirstName = person.FirstName;
            adminInDb.LastName = person.LastName;
            adminInDb.Password = person.Password; // Need hash later
            adminInDb.PhoneNumber = person.PhoneNumber;

            //* Check if the admin provide a photo
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

                //* Delete the previous image of the admin if he already has one
                if (!String.IsNullOrEmpty(adminInDb.Photo))
                {
                    Utilities.DeleteImageFromServer(adminInDb.Photo);
                }

                //* Get the path of the new photo
                var ImagePath = Utilities.GetPersonImagePath(userPhoto);

                // Save the image on the device 
                userPhoto.SaveAs(Server.MapPath(ImagePath));

                //* Set the path to the admin's photo field
                adminInDb.Photo = ImagePath;
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

        // Get: Dashboard/Posts/Pending
        [Route("Dashboard/posts/pending")]
        public ActionResult PendingPosts()
        {
            /* Get the pending posts only */
            var pendingPosts = _dbContext.Posts.Include(p => p.Agency).Where(y => y.Status == Post.PENDING);

            return View(pendingPosts);
        }

        // Get: Dashboard/Posts
        [Route("Dashboard/posts")]
        public ActionResult AllPosts()
        {
            /* Get all posts data */
            var allPosts = GetAllPosts();

            return View(allPosts);
        }

        [Route("Dashboard/posts/delete")] // Reference in AdminDeletePost.js
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

        [HttpPost]
        [Route("Dashboard/posts/approve")] // Reference in ChangePostStatus.js
        public JsonResult ApprovePost(int id)
        {
            var post = _dbContext.Posts.Single(p => p.Id == id);
            post.Status = Post.APPROVED;
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new { success = true, message = "The Trip Post Now On the Wall!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("Dashboard/posts/refuse")]// Reference in ChangePostStatus.js
        public JsonResult RefusePost(int id, string refuseMessage)
        {
            var post = _dbContext.Posts.Single(p => p.Id == id);
            post.Status = Post.REFUSED;
            post.RefuseMessage = refuseMessage;
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Refuse the trip post done successfully!" }, JsonRequestBehavior.AllowGet);
        }

        /* Helper Methods */

        // Return all users (agencies and travelers)
        public IEnumerable<Person> GetAllUsers()
        {
            var users = _dbContext.Users.Include(u => u.UserRole).ToList();
            return users;
        }

        // Return all users (agencies and travelers)
        public IEnumerable<Post> GetAllPosts()
        {
            var posts = _dbContext.Posts.Include(p => p.Agency).OrderBy(p => p.PostDate).ToList();
            return posts;
        }

        public WallViewModel GetWallViewModel()
        {
            // Get logged in admin
            var loggedInAdmin = AccountController.GetUserFromEmail(User.Identity.Name);

            var viewModel = new WallViewModel
            {
                User = loggedInAdmin
            };

            return viewModel;
        }
    }
}