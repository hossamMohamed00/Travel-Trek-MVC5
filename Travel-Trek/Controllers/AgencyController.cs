using System;
using System.Linq;
using System.Web.Mvc;
using Travel_Trek.Db_Context;
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
        public ActionResult Index()
        {
            return View();
        }

        // Get: Agency/Profile
        [Route("Agency/Profile")]
        public ActionResult Profile()
        {
            var agency = Db.Users.Include("UserRole").SingleOrDefault(u => u.Id == 2);

            var viewModel = new UserFormViewModel
            {
                User = agency
            };

            return View("UserProfile", viewModel);
        }

        [Route("Agency/Profile/Edit")]
        public ActionResult Edit()
        {
            var agency = Db.Users.Include("UserRole").SingleOrDefault(u => u.Id == 2);

            var viewModel = new UserFormViewModel
            {
                User = agency
            };

            return View("UserProfileEdit", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(UserFormViewModel viewModel)
        {
            var person = viewModel.User;

            if (!ModelState.IsValid)
            {
                var agency = Db.Users.Include("UserRole").SingleOrDefault(u => u.Id == 2);
                var userFormViewModelviewModel = new UserFormViewModel
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


    }
}