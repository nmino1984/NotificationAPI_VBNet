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
        var request = new SendNotificationRequest(Guid.NewGuid(), "Hello", "World message");
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_InvalidRequest_FailsValidation()
    {
        var request = new SendNotificationRequest(Guid.Empty, "", "");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Message);
    }
}
