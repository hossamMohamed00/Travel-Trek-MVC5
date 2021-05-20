using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel_Trek.Db_Context;
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
        public ActionResult Save(WallViewModel viewModel)
        {
            // Get user email
            var email = Db.Users.FirstOrDefault(u => u.Id == viewModel.User.Id)?.Email;
            viewModel.User.Email = email;

            var person = viewModel.User;

            if (!ModelState.IsValid)
            {
                var agency = Db.Users.Include("UserRole").SingleOrDefault(u => u.Id == 2);
                var userFormViewModelviewModel = new WallViewModel
                {
                    User = agency
                };

                return View("UserProfileEdit", userFormViewModelviewModel);
            }

            var agencyInDb = Db.Users.Single(m => m.Id == person.Id);
            agencyInDb.FirstName = person.FirstName;
            agencyInDb.LastName = person.LastName;
            agencyInDb.Password = person.Password; // Need hash later
            agencyInDb.PhoneNumber = person.PhoneNumber;
            //adminInDb.Photo = person.Photo; // Need change later

            try
            {
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return RedirectToAction("Profile", "Agency");
        }


        [Route("Agency/Posts/Create")]
        [Authorize(Roles = "Agency")]
        public ActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PublishPost(Post post, HttpPostedFileBase TripImage)
        {
            if (ModelState.IsValid)
            {
                string path = "";
                if (TripImage.FileName.Length > 0)
                {
                    path = "~/Content/images/posts" + Path.GetFileName(TripImage.FileName);
                    path = path + post.PostDate;// Add unique value to the 
                    TripImage.SaveAs(Server.MapPath(path));
                }

                post.TripImage = path;
                post.AgencyId = 2; // Need update later

                Db.Posts.Add(post);
                Db.SaveChanges();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Agency");
        }

        /* Helper Methods */
        public WallViewModel GetWallViewModel()
        {
            var agency = Db.Users.Include("UserRole").SingleOrDefault(u => u.Id == 2); // Need Edit later

            var viewModel = new WallViewModel
            {
                User = agency
            };

            return viewModel;
        }
    }
}