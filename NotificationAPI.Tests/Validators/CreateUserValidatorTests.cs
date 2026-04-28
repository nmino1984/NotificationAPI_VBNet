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
        var request = new CreateUserRequest("John Doe", "john@example.com");
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_InvalidRequest_FailsValidation()
    {
        var request = new CreateUserRequest("Jo", "not-an-email");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}
