using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Core.Entities;
using IdentityModel;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSqlServerDbAndIdentity(
        this IServiceCollection services,
        string connectionString,
        string identityConnectionString,
        IConfigurationSection identitySection 
    )
    {
        services.AddDbContext<AppDbContext>(builder =>
            builder.UseSqlServer(connectionString));
        
        services.AddTransient<IRepository<Book>, BookRepository>();

        services.AddDbContext<IdentityContext>(builder =>
            builder.UseSqlServer(identityConnectionString));
        services.AddDefaultIdentity<AppUser>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;
                })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        // clearing mapping for such claims as sub, cuz it can be named as schemas.xmlsoap.org...
        // and it wouldn't be recognized by User.Identity.GetSubjectId() [throws exception: sub claim not found]
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = identitySection["Authority"];
                options.SaveToken = true;

                options.TokenValidationParameters.ValidateAudience = false;
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.ValidateIssuerSigningKey = true;
                
                options.TokenValidationParameters.ValidIssuer = identitySection["Issuer"];
                options.TokenValidationParameters.IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identitySection["Secret"].ToSha256()));
            });

        return services;
    }
}