Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Mvc
Imports NotificationAPI.Application.DTOs.Requests
Imports NotificationAPI.Application.DTOs.Responses
Imports NotificationAPI.Application.UseCases.Notifications
Imports NotificationAPI.Domain.Repositories

Namespace NotificationAPI.API.Controllers

    <ApiController>
    <Route("api/[controller]")>
    Public Class NotificationsController
        Inherits ControllerBase

        Private _sendNotificationUseCase As SendNotificationUseCase
        Private _unitOfWork As IUnitOfWork

        Public Sub New(sendNotificationUseCase As SendNotificationUseCase, unitOfWork As IUnitOfWork)
            _sendNotificationUseCase = sendNotificationUseCase
            _unitOfWork = unitOfWork
        End Sub

        <HttpPost("send")>
        <ProducesResponseType(GetType(NotificationResponse), StatusCodes.Status201Created)>
        <ProducesResponseType(StatusCodes.Status400BadRequest)>
        Public Async Function SendNotification(<FromBody> request As SendNotificationRequest) As Task(Of IActionResult)
            Try
                Dim response = Await _sendNotificationUseCase.ExecuteAsync(request)
                Await _unitOfWork.SaveChangesAsync()
                Return CreatedAtAction(NameOf(GetNotificationById), New With {.id = response.Id}, response)
            Catch ex As InvalidOperationException
                Return BadRequest(New With {.error = ex.Message})
            Catch ex As Exception
                Return StatusCode(StatusCodes.Status500InternalServerError, New With {.error = "An unexpected error occurred"})
            End Try
        End Function

        <HttpGet("{id:guid}")>
        <ProducesResponseType(GetType(NotificationResponse), StatusCodes.Status200OK)>
        <ProducesResponseType(StatusCodes.Status404NotFound)>
        Public Async Function GetNotificationById(id As Guid) As Task(Of IActionResult)
            Try
                Dim notification = Await _unitOfWork.Notifications.GetByIdAsync(id)
                If notification Is Nothing Then
                    Return NotFound(New With {.error = $"Notification with ID {id} not found"})
                End If
                Return Ok(notification)
            Catch ex As Exception
                Return StatusCode(StatusCodes.Status500InternalServerError, New With {.error = "An unexpected error occurred"})
            End Try
        End Function

        <HttpGet("user/{userId:guid}")>
        <ProducesResponseType(GetType(IEnumerable(Of NotificationResponse)), StatusCodes.Status200OK)>
        Public Async Function GetNotificationsByUserId(userId As Guid) As Task(Of IActionResult)
            Try
                Dim notifications = Await _unitOfWork.Notifications.GetByUserIdAsync(userId)
                Return Ok(notifications)
            Catch ex As Exception
                Return StatusCode(StatusCodes.Status500InternalServerError, New With {.error = "An unexpected error occurred"})
            End Try
        End Function
    End Class

End Namespace
