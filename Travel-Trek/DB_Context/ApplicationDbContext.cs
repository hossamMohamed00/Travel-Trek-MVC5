﻿using System.Data.Entity;
using Travel_Trek.Models;

namespace Travel_Trek.Db_Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Person> Users { get; set; }

        public DbSet<Post> Posts { get; set; }
    }
}