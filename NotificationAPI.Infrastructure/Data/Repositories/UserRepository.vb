Imports NotificationAPI.Domain.Entities
Imports NotificationAPI.Domain.Repositories
Imports Microsoft.EntityFrameworkCore

Namespace NotificationAPI.Infrastructure.Data.Repositories

    Public Class UserRepository
        Implements IUserRepository

        Private _context As ApplicationDbContext

        Public Sub New(context As ApplicationDbContext)
            _context = context
        End Sub

        Public Async Function GetByIdAsync(id As Guid) As Task(Of User) Implements IUserRepository.GetByIdAsync
            Return Await _context.Users.AsNoTracking() _
                .FirstOrDefaultAsync(Function(u) u.Id = id And Not u.IsDeleted)
        End Function

        Public Async Function GetAllAsync() As Task(Of IEnumerable(Of User)) Implements IUserRepository.GetAllAsync
            Return Await _context.Users.AsNoTracking() _
                .Where(Function(u) Not u.IsDeleted) _
                .ToListAsync()
        End Function

        Public Async Function AddAsync(user As User) As Task Implements IUserRepository.AddAsync
            Await _context.Users.AddAsync(user)
        End Function

        Public Async Function UpdateAsync(user As User) As Task Implements IUserRepository.UpdateAsync
            user.UpdatedAt = DateTime.UtcNow
            _context.Users.Update(user)
            Await Task.CompletedTask
        End Function

        Public Async Function DeleteAsync(id As Guid) As Task Implements IUserRepository.DeleteAsync
            Dim user = Await GetByIdAsync(id)
            If user IsNot Nothing Then
                user.IsDeleted = True
                user.UpdatedAt = DateTime.UtcNow
                _context.Users.Update(user)
            End If
        End Function
    End Class

End Namespace
