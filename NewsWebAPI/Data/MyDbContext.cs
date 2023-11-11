using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Entities;

namespace NewsWebAPI.Data
{
    public class MyDbContext : DbContext
    {

        public MyDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<User> Users { get; set; }
    }
}
