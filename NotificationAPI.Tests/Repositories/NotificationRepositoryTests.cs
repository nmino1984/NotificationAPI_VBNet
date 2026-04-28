using Microsoft.EntityFrameworkCore;
using NotificationAPI.Domain.Entities;
using NotificationAPI.Infrastructure.Data;
using NotificationAPI.Infrastructure.Data.Repositories;

namespace NotificationAPI.Tests.Repositories;

public class NotificationRepositoryTests
{
    private static ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetByUserIdAsync_ReturnsOrderedByCreatedAtDescending()
    {
        var dbName = Guid.NewGuid().ToString();
        var userId = Guid.NewGuid();

        var older = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = "First",
            Message = "Message",
            CreatedAt = DateTime.UtcNow.AddHours(-1)
        };
        var newer = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = "Second",
            Message = "Message",
            CreatedAt = DateTime.UtcNow
        };

        using (var ctx = CreateContext(dbName))
        {
            await ctx.Notifications.AddRangeAsync(older, newer);
            await ctx.SaveChangesAsync();
        }

        using (var ctx = CreateContext(dbName))
        {
            var repo = new NotificationRepository(ctx);
            var results = (await repo.GetByUserIdAsync(userId)).ToList();
            Assert.Equal(2, results.Count);
            Assert.Equal(newer.Id, results[0].Id);
        }
    }

    [Fact]
    public async Task GetByUserIdAsync_FiltersDeletedNotifications()
    {
        var dbName = Guid.NewGuid().ToString();
        var userId = Guid.NewGuid();

        var active = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = "Active",
            Message = "Message",
            IsDeleted = false
        };
        var deleted = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = "Deleted",
            Message = "Message",
            IsDeleted = true
        };

        using (var ctx = CreateContext(dbName))
        {
            await ctx.Notifications.AddRangeAsync(active, deleted);
            await ctx.SaveChangesAsync();
        }

        using (var ctx = CreateContext(dbName))
        {
            var repo = new NotificationRepository(ctx);
            var results = (await repo.GetByUserIdAsync(userId)).ToList();
            Assert.Single(results);
            Assert.Equal(active.Id, results[0].Id);
        }
    }
}
