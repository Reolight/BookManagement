using Core.Entities;
using Infrastructure.Converters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Book> Books = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) => CreateDb();

    public AppDbContext() => CreateDb();

    private void CreateDb() =>
        Database.EnsureCreated();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // even without this line EF will find PK correctly by it's name 
        modelBuilder.Entity<Book>()
            .HasKey(book => book.Id);

        // isbn has 13 digits max (hyphens must be omitted)
        modelBuilder.Entity<Book>()
            .Property(book => book.Isbn)
            // isbn has 13 digits and 4 delimiters (hyphens usually)
            .HasMaxLength(17);

        modelBuilder.Entity<Book>()
            .Property(book => book.Author)
            .HasMaxLength(64);

        modelBuilder.Entity<Book>()
            .Property(book => book.Genre)
            .HasMaxLength(64);
        
        modelBuilder.Entity<Book>()
            .Property(book => book.Name)
            .HasMaxLength(128);
        
        // some people search books by isbn 
        modelBuilder.Entity<Book>()
            .HasIndex(book => book.Isbn);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        // DateOnly will get support out of the box in EF8. For now converter is used. 
        configurationBuilder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter, DateOnlyComparer>()
            .HaveColumnType("date");
    }
}