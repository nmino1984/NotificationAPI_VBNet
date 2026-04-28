using Microsoft.EntityFrameworkCore;
using NotificationAPI.Application.Mappings;
using NotificationAPI.Application.UseCases.Notifications;
using NotificationAPI.Application.UseCases.Users;
using NotificationAPI.Application.Validators;
using NotificationAPI.Domain.Repositories;
using NotificationAPI.Infrastructure.Data;
using NotificationAPI.Infrastructure.Data.Repositories;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories and Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// Use Cases
builder.Services.AddScoped<SendNotificationUseCase>();
builder.Services.AddScoped<GetUserByIdUseCase>();

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialize database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // HTTPS redirect only in development — production SSL is terminated by the reverse proxy (Railway/nginx)
    app.UseHttpsRedirection();
}

// Health check endpoint for container orchestration
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.UseAuthorization();
app.MapControllers();

app.Run();
