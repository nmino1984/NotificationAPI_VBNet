Imports NotificationAPI.Application.DTOs.Responses
Imports NotificationAPI.Domain.Repositories
Imports AutoMapper

Namespace NotificationAPI.Application.UseCases.Users

    Public Class GetUserByIdUseCase
        Private _userRepository As IUserRepository
        Private _mapper As IMapper

        Public Sub New(userRepository As IUserRepository, mapper As IMapper)
            _userRepository = userRepository
            _mapper = mapper
        End Sub

        Public Async Function ExecuteAsync(userId As Guid) As Task(Of UserResponse)
            Dim user = Await _userRepository.GetByIdAsync(userId)
            If user Is Nothing Then
                Throw New InvalidOperationException($"User with ID {userId} not found")
            End If

            Dim response = _mapper.Map(Of UserResponse)(user)
            Return response
        End Function
    End Class

End Namespace
