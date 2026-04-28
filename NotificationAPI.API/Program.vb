Imports Microsoft.AspNetCore.Builder
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports Swashbuckle.AspNetCore.SwaggerUI
Imports AutoMapper
Imports NotificationAPI.Application.Mappings
Imports NotificationAPI.Application.Validators
Imports NotificationAPI.Application.UseCases.Notifications
Imports NotificationAPI.Application.UseCases.Users
Imports NotificationAPI.Domain.Repositories
Imports NotificationAPI.Infrastructure.Data
Imports NotificationAPI.Infrastructure.Data.Repositories
Imports FluentValidation

Module Program
    Sub Main(args As String())
        Dim builder = WebApplication.CreateBuilder(args)

        ' DbContext
        Dim dbOptions As Action(Of DbContextOptionsBuilder) =
            Sub(options As DbContextOptionsBuilder)
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
            End Sub
        builder.Services.AddDbContext(Of ApplicationDbContext)(dbOptions)

        ' Repositories and Unit of Work
        builder.Services.AddScoped(Of IUnitOfWork, UnitOfWork)
        builder.Services.AddScoped(Of IUserRepository, UserRepository)
        builder.Services.AddScoped(Of INotificationRepository, NotificationRepository)

        ' Use Cases
        builder.Services.AddScoped(Of SendNotificationUseCase)
        builder.Services.AddScoped(Of GetUserByIdUseCase)

        ' AutoMapper
        builder.Services.AddAutoMapper(Sub(cfg) cfg.AddProfile(New MappingProfile()))

        ' FluentValidation
        builder.Services.AddValidatorsFromAssemblyContaining(Of CreateUserValidator)()

        ' Controllers
        builder.Services.AddControllers()

        ' Swagger
        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen()

        Dim app = builder.Build()

        ' Initialize database on startup
        Using scope = app.Services.CreateScope()
            Dim db = scope.ServiceProvider.GetRequiredService(Of ApplicationDbContext)()
            db.Database.EnsureCreated()
        End Using

        If app.Environment.IsDevelopment() Then
            app.UseSwagger()
            app.UseSwaggerUI()
        End If

        app.UseHttpsRedirection()
        app.UseAuthorization()
        app.MapControllers()

        app.Run()
    End Sub
End Module
