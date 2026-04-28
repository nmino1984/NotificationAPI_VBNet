using AutoMapper;
using Moq;
using NotificationAPI.Application.DTOs.Requests;
using NotificationAPI.Application.DTOs.Responses;
using NotificationAPI.Application.UseCases.Notifications;
using NotificationAPI.Domain.Entities;
using NotificationAPI.Domain.Repositories;

namespace NotificationAPI.Tests.UseCases;

public class SendNotificationUseCaseTests
{
    private readonly Mock<INotificationRepository> _notifRepoMock = new();
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly SendNotificationUseCase _useCase;

    public SendNotificationUseCaseTests()
    {
        _useCase = new SendNotificationUseCase(_notifRepoMock.Object, _userRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ValidRequest_ReturnsNotificationResponse()
    {
        var userId = Guid.NewGuid();
        var notifId = Guid.NewGuid();
        var request = new SendNotificationRequest(userId, "Hello", "World message");
        var user = new User { Id = userId, Name = "John", Email = "john@example.com" };
        var notification = new Notification { Id = notifId, UserId = userId, Title = "Hello", Message = "World message" };
        var expected = new NotificationResponse { Id = notifId, UserId = userId, Title = "Hello", Message = "World message" };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<Notification>(request)).Returns(notification);
        _mapperMock.Setup(m => m.Map<NotificationResponse>(notification)).Returns(expected);
        _notifRepoMock.Setup(r => r.AddAsync(notification)).Returns(Task.CompletedTask);

        var result = await _useCase.ExecuteAsync(request);

        Assert.Equal(notifId, result.Id);
        Assert.Equal("Hello", result.Title);
    }

    [Fact]
    public async Task ExecuteAsync_UserNotFound_ThrowsInvalidOperationException()
    {
        var userId = Guid.NewGuid();
        var request = new SendNotificationRequest(userId, "Hello", "World message");
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(request));
    }
}
