Namespace NotificationAPI.Application.DTOs.Requests

    Public Class CreateUserRequest
        Public Property Name As String
        Public Property Email As String

        Public Sub New()
        End Sub

        Public Sub New(name As String, email As String)
            Me.Name = name
            Me.Email = email
        End Sub
    End Class

End Namespace
