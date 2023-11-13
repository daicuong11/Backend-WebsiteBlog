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
        public DbSet<Image> Images { get; set; }

        public DbSet<ImageArticleMapping> ImageArticleMappings {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
            .HasOne(c => c.Article)
            .WithMany(a => a.Comments)
            .HasForeignKey(c => c.ArticleID)
            .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Like>()
            .HasOne(l => l.Article)
            .WithMany(a => a.Likes)
            .HasForeignKey(l => l.ArticleID)
            .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
