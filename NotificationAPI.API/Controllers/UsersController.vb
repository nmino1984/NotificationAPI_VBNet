Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Mvc
Imports NotificationAPI.Application.DTOs.Requests
Imports NotificationAPI.Application.DTOs.Responses
Imports NotificationAPI.Application.UseCases.Users
Imports NotificationAPI.Domain.Entities
Imports NotificationAPI.Domain.Repositories
Imports AutoMapper

Namespace NotificationAPI.API.Controllers

    <ApiController>
    <Route("api/[controller]")>
    Public Class UsersController
        Inherits ControllerBase

        Private _unitOfWork As IUnitOfWork
        Private _mapper As IMapper
        Private _getUserByIdUseCase As GetUserByIdUseCase

        Public Sub New(unitOfWork As IUnitOfWork, mapper As IMapper, getUserByIdUseCase As GetUserByIdUseCase)
            _unitOfWork = unitOfWork
            _mapper = mapper
            _getUserByIdUseCase = getUserByIdUseCase
        End Sub

        <HttpPost>
        <ProducesResponseType(GetType(UserResponse), StatusCodes.Status201Created)>
        <ProducesResponseType(StatusCodes.Status400BadRequest)>
        Public Async Function CreateUser(<FromBody> request As CreateUserRequest) As Task(Of IActionResult)
            Try
                Dim user = _mapper.Map(Of User)(request)
                Await _unitOfWork.Users.AddAsync(user)
                Await _unitOfWork.SaveChangesAsync()

                Dim response = _mapper.Map(Of UserResponse)(user)
                Return CreatedAtAction(NameOf(GetUserById), New With {.id = response.Id}, response)
            Catch ex As Exception
                Return StatusCode(StatusCodes.Status500InternalServerError, New With {.error = "An unexpected error occurred"})
            End Try
        End Function

        <HttpGet("{id:guid}")>
        <ProducesResponseType(GetType(UserResponse), StatusCodes.Status200OK)>
        <ProducesResponseType(StatusCodes.Status404NotFound)>
        Public Async Function GetUserById(id As Guid) As Task(Of IActionResult)
            Try
                Dim response = Await _getUserByIdUseCase.ExecuteAsync(id)
                Return Ok(response)
            Catch ex As InvalidOperationException
                Return NotFound(New With {.error = ex.Message})
            Catch ex As Exception
                Return StatusCode(StatusCodes.Status500InternalServerError, New With {.error = "An unexpected error occurred"})
            End Try
        End Function

        <HttpGet>
        <ProducesResponseType(GetType(IEnumerable(Of UserResponse)), StatusCodes.Status200OK)>
        Public Async Function GetAllUsers() As Task(Of IActionResult)
            Try
                Dim users = Await _unitOfWork.Users.GetAllAsync()
                Dim responses = _mapper.Map(Of IEnumerable(Of UserResponse))(users)
                Return Ok(responses)
            Catch ex As Exception
                Return StatusCode(StatusCodes.Status500InternalServerError, New With {.error = "An unexpected error occurred"})
            End Try
        End Function
    End Class

End Namespace
