using System;
using System.IO;
using System.Linq;
using System.Web;
using Travel_Trek.Db_Context;

namespace Travel_Trek.Helpers
{
    public class Utilities
    {
        /*
         * Return post image path
         */
        public static string GetPostImagePath(HttpPostedFileBase TripImage, DateTime PostDate)
        {
            string fileName = TripImage.FileName;
            if (fileName.Length > 0)
            {
                // Add unique value (PostDate) to the path
                var imageNameWithExtension = fileName.Split('.'); // Split the fileName 

                var suffix = PostDate.ToString("yyyyMMddHHmmssffff"); //Convert the value to this format

                var fileNameWithSuffix = imageNameWithExtension[0] + suffix + '.' + imageNameWithExtension[1];

                string path = "/Content/images/posts/" + fileNameWithSuffix;

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


        /*
         * Delete image from the server
         */
        public static void DeleteImageFromServer(string imagePath)
        {
            // imagePath In format like this -> /Content/images/{posts/users}}/{postImage}
            string fullPath = HttpContext.Current.Request.MapPath("~" + imagePath);

            //* If the file exists, then delete it
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);

            }
        }

        /**
         * Delete post
         */
        public static bool DeletePostFromDb(int? id, ApplicationDbContext db)
        {
            //* Delete Post
            var post = db.Posts.Single(p => p.Id == id);

            if (post == null)
                return false;

            //* Remove the post from saved and likes posts tables
            var savedPosts = db.SavedPosts.Where(p => p.PostId == id);
            db.SavedPosts.RemoveRange(savedPosts);

            var postLikes = db.LikedPosts.Where(p => p.PostId == id);
            db.LikedPosts.RemoveRange(postLikes);

            //* Remove post image from the device
            if (!string.IsNullOrEmpty(post.TripImage))
            {
                DeleteImageFromServer(post.TripImage);
            }

            //* Remove the post from the db
            db.Posts.Remove(post);

            // Save the changes to the db
            db.SaveChanges();

            return true;
        }

        /**
         * Validate the image extinsions
         */
        public static bool ValidateImageExtension(HttpPostedFileBase image)
        {
            string[] ALLOWED_EXTENSION = new string[3] { "png", "jpg", "jpeg" };

            var extension = Path.GetExtension(image.FileName).Substring(1); // to get the extension without the (.)

            var isExists = ALLOWED_EXTENSION.Contains(extension);

            return isExists;
        }
    }
}