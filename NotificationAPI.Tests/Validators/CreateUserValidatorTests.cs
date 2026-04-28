using FluentValidation.TestHelper;
using NotificationAPI.Application.DTOs.Requests;
using NotificationAPI.Application.Validators;

namespace NotificationAPI.Tests.Validators;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator = new();

    [Fact]
    public void Validate_ValidRequest_PassesValidation()
    {
        // Arrange
        var request = new CreateUserRequest("John Doe", "john@example.com");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_InvalidRequest_FailsValidation()
    {
        // Arrange — name too short (< 3 chars), invalid email format
        var request = new CreateUserRequest("Jo", "not-an-email");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}
