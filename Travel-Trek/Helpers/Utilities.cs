using System;
using System.Web;

namespace Travel_Trek.Helpers
{
    public class Utilities
    {
        /*
         * Return post image path
         */
        public static string GetPostImagePath(HttpPostedFileBase TripImage, DateTime PostDate)
        {
            string path = "";
            string fileName = TripImage.FileName;
            if (fileName.Length > 0)
            {
                // Add unique value (PostDate) to the path
                var imageNameWithExtension = fileName.Split('.'); // Split the fileName 

                var suffix = PostDate.ToString("yyyyMMddHHmmssffff"); //Convert the value to this format

                var fileNameWithSuffix = imageNameWithExtension[0] + suffix + '.' + imageNameWithExtension[1];

                path = "/Content/images/posts/" + fileNameWithSuffix;

                return path;
            }

            return "No Image Provided!";
        }

        /*
         * Return person image path
         */
        public static string GetPersonImagePath(HttpPostedFileBase userPhoto)
        {
            string path;
            string fileName = userPhoto.FileName;
            if (fileName.Length > 0)
            {
                // Add unique value (DateTime.Now) to the path
                var imageNameWithExtension = fileName.Split('.'); // Split the fileName 

                var suffix = DateTime.Now.ToString("yyyyMMddHHmmssffff"); // Convert the value to this format

                var fileNameWithSuffix = imageNameWithExtension[0] + suffix + '.' + imageNameWithExtension[1];

                path = "/Content/images/users/" + fileNameWithSuffix;

                return path;
            }

            return "/Content/images/users/default.png"; // return Default
        }


    }
}