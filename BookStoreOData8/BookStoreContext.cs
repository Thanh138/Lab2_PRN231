using BookStoreOData8.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreOData8
{
    public class BookStoreContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Press> Presses { get; set; }

        // Constructor
        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define Address as an owned type within Book
            modelBuilder.Entity<Book>().OwnsOne(b => b.Location);

            // Configure the relationship between Book and Press
            modelBuilder.Entity<Book>()
       .HasOne(b => b.Press)
       .WithMany()
       .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
