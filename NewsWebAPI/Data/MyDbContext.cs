using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Entities;

namespace NewsWebAPI.Data
{
    public class MyDbContext : DbContext
    {

        public MyDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}
