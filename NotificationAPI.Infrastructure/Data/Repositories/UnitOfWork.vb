Imports NotificationAPI.Domain.Repositories

Namespace NotificationAPI.Infrastructure.Data.Repositories

    Public Class UnitOfWork
        Implements IUnitOfWork

        Private _context As ApplicationDbContext
        Private _users As IUserRepository
        Private _notifications As INotificationRepository

        Public Sub New(context As ApplicationDbContext)
            _context = context
        End Sub

        Public ReadOnly Property Users As IUserRepository Implements IUnitOfWork.Users
            Get
                If _users Is Nothing Then
                    _users = New UserRepository(_context)
                End If
                Return _users
            End Get
        End Property

        Public ReadOnly Property Notifications As INotificationRepository Implements IUnitOfWork.Notifications
            Get
                If _notifications Is Nothing Then
                    _notifications = New NotificationRepository(_context)
                End If
                Return _notifications
            End Get
        End Property

        Public Async Function SaveChangesAsync() As Task(Of Integer) Implements IUnitOfWork.SaveChangesAsync
            Return Await _context.SaveChangesAsync()
        End Function

        Public Sub Dispose() Implements IUnitOfWork.Dispose
            _context?.Dispose()
        End Sub
    End Class

End Namespace
