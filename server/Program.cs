using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Server.API.Shared.Persistence.Repositories;
using Server.API.Shared.Domain.Repositories;
using Server.API.Shared.Persistence.Context;
using Server.API.Shared.Middleware;
using Server.API.Server.Domain.Repositories;
using Server.API.Server.Persistence;
using Server.API.Server.Services;
using Server.API.Server.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Swagger Settings
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ServerAPI",
        Description = "Server API Web Services",
        Contact = new OpenApiContact
        {
            Name = "Server.studio",
            Url = new Uri("https://github.com")
        }

    });
    options.EnableAnnotations();
    options.SupportNonNullableReferenceTypes();
});


// Database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(connectionString)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors());

// Add lower case urls
builder.Services.AddRouting(
    options => options.LowercaseUrls = true);

// Add cors
builder.Services.AddCors();

// Configuration
// ---  Dependency Injection Configuration --- //

// Shared injection conifguration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Auth Injection Configuration
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Secutiry intection configuration
// ...


//AutoMapper configuration
builder.Services.AddAutoMapper(
    typeof(Server.API.Server.Mapping.ModelToResource),
    typeof(Server.API.Server.Mapping.SaveResourceToModel));

var app = builder.Build();

// Validation for ensuring database objects are created.
using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<AppDbContext>())
{
    context.Database.EnsureCreated();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowAnyHeader());



app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
