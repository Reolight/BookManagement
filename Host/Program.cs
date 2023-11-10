using Application.Services;
using Application.Services.Abstractions;
using Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
string dbConnectionString = builder.Configuration.GetConnectionString("dbConnectionString") 
                            ?? throw new ArgumentNullException(nameof(dbConnectionString), "Connection string to db is not specified");
string identityConnectionString = builder.Configuration.GetConnectionString(nameof(identityConnectionString))
                                  ?? throw new ArgumentNullException(nameof(identityConnectionString),
                                      "identity connection string is not specified");
var identitySection = builder.Configuration.GetSection("Identity");

// Add services to the container.
builder.Services.AddSqlServerDbAndIdentity(dbConnectionString, identityConnectionString, identitySection);

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                            
                In = ParameterLocation.Header
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();