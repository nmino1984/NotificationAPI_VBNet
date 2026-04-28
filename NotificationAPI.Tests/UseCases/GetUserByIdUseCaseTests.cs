using AutoMapper;
using Moq;
using NotificationAPI.Application.DTOs.Responses;
using NotificationAPI.Application.UseCases.Users;
using NotificationAPI.Domain.Entities;
using NotificationAPI.Domain.Repositories;

namespace NotificationAPI.Tests.UseCases;

public class GetUserByIdUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly GetUserByIdUseCase _useCase;

    public GetUserByIdUseCaseTests()
    {
        _useCase = new GetUserByIdUseCase(_userRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_UserExists_ReturnsUserResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Name = "John Doe", Email = "john@example.com" };
        var expected = new UserResponse { Id = userId, Name = "John Doe", Email = "john@example.com" };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(expected);

        // Act
        var result = await _useCase.ExecuteAsync(userId);

        // Assert
        Assert.Equal(userId, result.Id);
        Assert.Equal("John Doe", result.Name);
    }

    [Fact]
    public async Task ExecuteAsync_UserNotFound_ThrowsInvalidOperationException()
    {
        // Arrange — repository returns null (user does not exist)
        var userId = Guid.NewGuid();
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(userId));
    }
}
