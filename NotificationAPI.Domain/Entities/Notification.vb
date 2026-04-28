Namespace NotificationAPI.Domain.Entities

    Public Class Notification
        Public Property Id As Guid
        Public Property UserId As Guid
        Public Property Title As String
        Public Property Message As String
        Public Property IsRead As Boolean
        Public Property CreatedAt As DateTime
        Public Property UpdatedAt As DateTime
        Public Property IsDeleted As Boolean
        Public Property User As User

        Public Sub New()
        End Sub

        Public Sub New(userId As Guid, title As String, message As String)
            Id = Guid.NewGuid()
            Me.UserId = userId
            Me.Title = title
            Me.Message = message
            IsRead = False
            CreatedAt = DateTime.UtcNow
            UpdatedAt = DateTime.UtcNow
            IsDeleted = False
        End Sub
    End Class

End Namespace
