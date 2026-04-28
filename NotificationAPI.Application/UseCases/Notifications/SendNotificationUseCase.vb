Imports NotificationAPI.Application.DTOs.Requests
Imports NotificationAPI.Application.DTOs.Responses
Imports NotificationAPI.Domain.Entities
Imports NotificationAPI.Domain.Repositories
Imports AutoMapper

Namespace NotificationAPI.Application.UseCases.Notifications

    Public Class SendNotificationUseCase
        Private _notificationRepository As INotificationRepository
        Private _userRepository As IUserRepository
        Private _mapper As IMapper

        Public Sub New(notificationRepository As INotificationRepository,
                       userRepository As IUserRepository,
                       mapper As IMapper)
            _notificationRepository = notificationRepository
            _userRepository = userRepository
            _mapper = mapper
        End Sub

        Public Async Function ExecuteAsync(request As SendNotificationRequest) As Task(Of NotificationResponse)
            Dim user = Await _userRepository.GetByIdAsync(request.UserId)
            If user Is Nothing Then
                Throw New InvalidOperationException($"User with ID {request.UserId} not found")
            End If

            Dim notification = _mapper.Map(Of Notification)(request)

            Await _notificationRepository.AddAsync(notification)

            Dim response = _mapper.Map(Of NotificationResponse)(notification)
            Return response
        End Function
    End Class

End Namespace
