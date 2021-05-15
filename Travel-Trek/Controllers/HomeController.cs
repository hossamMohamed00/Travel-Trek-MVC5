using System.Web.Mvc;

namespace Travel_Trek.Controllers
{
      public class HomeController : Controller
      {
            public ActionResult Index()
            {
                  return View();
            }

            public ActionResult About()
            {
                  ViewBag.Message = "Remember those times when you had to search the newspapers and magazines to find out new vacation trips and journeys news to go. You always need to go to new places that you have not visited before with your friends. So we need a trip web application that allows you to find your best vacation trips to go that suits you and your friends and manage hotels, flights, buses, trains, etc. of the trip for you..";

                  return View();
            }


      }
}