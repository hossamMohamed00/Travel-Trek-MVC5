using System.ComponentModel.DataAnnotations;
using System.Web;
using Travel_Trek.Models;

namespace Travel_Trek.ViewModels
{
    public class PostFormViewModel
    {
        public Post Post { get; set; }

        public string Action { get; set; }

        public string Operation { get; set; }

        public string Title { get; set; }

        public string Header { get; set; }

        [Required(ErrorMessage = "The Trip Image is required")]
        [Display(Name = "Trip Image")]
        public HttpPostedFileBase TripImage { get; set; }
    }
}