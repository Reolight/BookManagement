using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class IdentityContext : IdentityDbContext<AppUser>
{
    public IdentityContext(DbContextOptions options) : base(options) => CreateDb();

    private void CreateDb() => Database.EnsureCreated();
}