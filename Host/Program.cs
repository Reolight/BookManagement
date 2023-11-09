using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
string dbConnectionString = builder.Configuration.GetConnectionString("dbConnectionString") 
                            ?? throw new ArgumentNullException(nameof(dbConnectionString), "Connection string to db is not specified");
string identityConnectionString = builder.Configuration.GetConnectionString(nameof(identityConnectionString))
                                  ?? throw new ArgumentNullException(nameof(identityConnectionString),
                                      "identity connection string is not specified");
var identitySection = builder.Configuration.GetSection("Identity");
// Add services to the container.

builder.Services.AddSqlServerDbAndIdentity(dbConnectionString, identityConnectionString, identitySection);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();