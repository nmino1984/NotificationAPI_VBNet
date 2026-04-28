using Microsoft.EntityFrameworkCore;
using NotificationAPI.Domain.Entities;
using NotificationAPI.Infrastructure.Data;
using NotificationAPI.Infrastructure.Data.Repositories;

namespace NotificationAPI.Tests.Infrastructure;

public class UnitOfWorkTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task SaveChangesAsync_PersistsChangesToDatabase()
    {
        var context = CreateContext();
        var uow = new UnitOfWork(context);
        var user = new User("Jane Doe", "jane@example.com");

        await uow.Users.AddAsync(user);
        await uow.SaveChangesAsync();

        var saved = await context.Users.FindAsync(user.Id);
        Assert.NotNull(saved);
        Assert.Equal("Jane Doe", saved.Name);
    }

    [Fact]
    public void Users_LazyInitialization_ReturnsSameInstance()
    {
        var context = CreateContext();
        var uow = new UnitOfWork(context);

        var first = uow.Users;
        var second = uow.Users;

        Assert.Same(first, second);
    }
}
