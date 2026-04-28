using Microsoft.EntityFrameworkCore;
using NotificationAPI.Domain.Entities;
using NotificationAPI.Infrastructure.Data;
using NotificationAPI.Infrastructure.Data.Repositories;

namespace NotificationAPI.Tests.Repositories;

public class UserRepositoryTests
{
    private static ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task DeleteAsync_SoftDeletesUser_UserNotFoundAfterDelete()
    {
        // Arrange — seed a user in an isolated in-memory database
        var dbName = Guid.NewGuid().ToString();
        var user = new User("Alice", "alice@example.com");

        using (var ctx = CreateContext(dbName))
        {
            await ctx.Users.AddAsync(user);
            await ctx.SaveChangesAsync();
        }

        // Act — soft delete the user using a fresh context instance
        using (var ctx = CreateContext(dbName))
        {
            var repo = new UserRepository(ctx);
            await repo.DeleteAsync(user.Id);
            await ctx.SaveChangesAsync();
        }

        // Assert — GetByIdAsync filters IsDeleted = true, so it returns null
        using (var ctx = CreateContext(dbName))
        {
            var repo = new UserRepository(ctx);
            var result = await repo.GetByIdAsync(user.Id);
            Assert.Null(result);
        }
    }

    [Fact]
    public async Task GetAllAsync_FiltersDeletedUsers()
    {
        // Arrange — seed two users, then soft delete one
        var dbName = Guid.NewGuid().ToString();
        var user1 = new User("Alice", "alice@example.com");
        var user2 = new User("Bob", "bob@example.com");

        using (var ctx = CreateContext(dbName))
        {
            await ctx.Users.AddRangeAsync(user1, user2);
            await ctx.SaveChangesAsync();
        }

        using (var ctx = CreateContext(dbName))
        {
            var repo = new UserRepository(ctx);
            await repo.DeleteAsync(user2.Id);
            await ctx.SaveChangesAsync();
        }

        // Act & Assert — only the non-deleted user is returned
        using (var ctx = CreateContext(dbName))
        {
            var repo = new UserRepository(ctx);
            var results = (await repo.GetAllAsync()).ToList();
            Assert.Single(results);
            Assert.Equal(user1.Id, results[0].Id);
        }
    }
}
