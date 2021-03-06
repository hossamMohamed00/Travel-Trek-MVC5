using System.Data.Entity;
using Travel_Trek.Models;

namespace Travel_Trek.Db_Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Person> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<SavedPosts> SavedPosts { get; set; }

        public DbSet<LikedPosts> LikedPosts { get; set; }

        public DbSet<DisLikedPosts> DisLikedPosts { get; set; }

        public DbSet<UserQuestion> UserQuestions { get; set; }

    }
}