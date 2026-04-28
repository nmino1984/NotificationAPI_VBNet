Namespace NotificationAPI.Application.DTOs.Responses

    Public Class NotificationResponse
        Public Property Id As Guid
        Public Property UserId As Guid
        Public Property Title As String
        Public Property Message As String
        Public Property IsRead As Boolean
        Public Property CreatedAt As DateTime
    End Class

End Namespace
