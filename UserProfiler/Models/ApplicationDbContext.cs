using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserProfiler.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<AnonymousUser> AnonymousUsers { get; set; }

        public DbSet<UserActivity> UserActivities { get; set; }

        public DbSet<ContentPage> ContentPages { get; set; }
    }
}
