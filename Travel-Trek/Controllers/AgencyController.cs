using System.Linq;
using System.Web.Mvc;
using Travel_Trek.Db_Context;
using Travel_Trek.Models;

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
        // Get: Dashboard/Profile
        public ActionResult Profile()
        {
            var Agency = Db.Users.SingleOrDefault(u => u.Id == 2); 
            return View(Agency);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Person person)
        {
            if (!ModelState.IsValid)
            {
                var agency = Db.Users.SingleOrDefault(u => u.Id == 2); 
                return View("Profile", agency);
            }

            var agencyInDb = Db.Users.Single(m => m.Id == person.Id);
            agencyInDb.FirstName = person.FirstName;
            agencyInDb.LastName = person.LastName;
            agencyInDb.Password = person.Password; // Need hash later
            agencyInDb.PhoneNumber = person.PhoneNumber;
            //adminInDb.Photo = person.Photo; // Need change later

            Db.SaveChanges();
            return RedirectToAction("Profile", "Agency");

        }


    }
}