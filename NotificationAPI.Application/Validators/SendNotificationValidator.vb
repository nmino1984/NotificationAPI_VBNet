Imports FluentValidation
Imports NotificationAPI.Application.DTOs.Requests

Namespace NotificationAPI.Application.Validators

    Public Class SendNotificationValidator
        Inherits AbstractValidator(Of SendNotificationRequest)

        Public Sub New()
            RuleFor(Function(x) x.UserId) _
                .NotEmpty().WithMessage("UserId is required")

            RuleFor(Function(x) x.Title) _
                .NotEmpty().WithMessage("Title is required") _
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters")

            RuleFor(Function(x) x.Message) _
                .NotEmpty().WithMessage("Message is required") _
                .MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters")
        End Sub
    End Class

End Namespace
