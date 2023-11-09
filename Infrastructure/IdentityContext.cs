using Core.Entities;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public class IdentityContext : ApiAuthorizationDbContext<AppUser>
{
    public IdentityContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) 
        : base(options, operationalStoreOptions)
    {
    }
}