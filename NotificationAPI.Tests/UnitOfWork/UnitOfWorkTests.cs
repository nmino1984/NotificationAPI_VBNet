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
        // Arrange
        var context = CreateContext();
        var uow = new UnitOfWork(context);
        var user = new User("Jane Doe", "jane@example.com");

        // Act
        await uow.Users.AddAsync(user);
        await uow.SaveChangesAsync();

        // Assert — entity is retrievable directly from the context
        var saved = await context.Users.FindAsync(user.Id);
        Assert.NotNull(saved);
        Assert.Equal("Jane Doe", saved.Name);
    }

    [Fact]
    public void Users_LazyInitialization_ReturnsSameInstance()
    {
        // Arrange
        var context = CreateContext();
        var uow = new UnitOfWork(context);

        // Act — access the property twice
        var first = uow.Users;
        var second = uow.Users;

        // Assert — same object reference (lazy init caches the instance)
        Assert.Same(first, second);
    }
}
