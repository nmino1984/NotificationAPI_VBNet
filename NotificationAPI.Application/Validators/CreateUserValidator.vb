Imports FluentValidation
Imports NotificationAPI.Application.DTOs.Requests

Namespace NotificationAPI.Application.Validators

    Public Class CreateUserValidator
        Inherits AbstractValidator(Of CreateUserRequest)

        Public Sub New()
            RuleFor(Function(x) x.Name) _
                .NotEmpty().WithMessage("Name is required") _
                .MinimumLength(3).WithMessage("Name must be at least 3 characters") _
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters")

            RuleFor(Function(x) x.Email) _
                .NotEmpty().WithMessage("Email is required") _
                .EmailAddress().WithMessage("Email must be valid")
        End Sub
    End Class

End Namespace
