Namespace NotificationAPI.Domain.Entities

    Public Class User
        Public Property Id As Guid
        Public Property Name As String
        Public Property Email As String
        Public Property CreatedAt As DateTime
        Public Property UpdatedAt As DateTime
        Public Property IsDeleted As Boolean

        Public Sub New()
        End Sub

        Public Sub New(name As String, email As String)
            Id = Guid.NewGuid()
            Me.Name = name
            Me.Email = email
            CreatedAt = DateTime.UtcNow
            UpdatedAt = DateTime.UtcNow
            IsDeleted = False
        End Sub
    End Class

End Namespace
