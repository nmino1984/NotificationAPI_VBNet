using FluentValidation.TestHelper;
using NotificationAPI.Application.DTOs.Requests;
using NotificationAPI.Application.Validators;

namespace NotificationAPI.Tests.Validators;

public class SendNotificationValidatorTests
{
    private readonly SendNotificationValidator _validator = new();

    [Fact]
    public void Validate_ValidRequest_PassesValidation()
    {
        // Arrange
        var request = new SendNotificationRequest(Guid.NewGuid(), "Hello", "World message");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_InvalidRequest_FailsValidation()
    {
        // Arrange — empty UserId (Guid.Empty), empty title and message
        var request = new SendNotificationRequest(Guid.Empty, "", "");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Message);
    }
}
