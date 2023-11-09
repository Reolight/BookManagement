using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Book> Books = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasKey(book => book.Id);
        
        modelBuilder.Entity<Book>()
            .HasIndex(book => book.Isbn);
    }
}