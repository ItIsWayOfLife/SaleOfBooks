using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartBooks> CartBooks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderBooks> OrderBooks { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
    }
}
