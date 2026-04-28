Namespace NotificationAPI.Application.DTOs.Requests

    Public Class SendNotificationRequest
        Public Property UserId As Guid
        Public Property Title As String
        Public Property Message As String

        Public Sub New()
        End Sub

        Public Sub New(userId As Guid, title As String, message As String)
            Me.UserId = userId
            Me.Title = title
            Me.Message = message
        End Sub
    End Class

End Namespace
