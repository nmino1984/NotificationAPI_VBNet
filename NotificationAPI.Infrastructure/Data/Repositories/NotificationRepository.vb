Imports NotificationAPI.Domain.Entities
Imports NotificationAPI.Domain.Repositories
Imports Microsoft.EntityFrameworkCore

Namespace NotificationAPI.Infrastructure.Data.Repositories

    Public Class NotificationRepository
        Implements INotificationRepository

        Private _context As ApplicationDbContext

        Public Sub New(context As ApplicationDbContext)
            _context = context
        End Sub

        Public Async Function GetByIdAsync(id As Guid) As Task(Of Notification) Implements INotificationRepository.GetByIdAsync
            Return Await _context.Notifications.AsNoTracking() _
                .FirstOrDefaultAsync(Function(n) n.Id = id And Not n.IsDeleted)
        End Function

        Public Async Function GetByUserIdAsync(userId As Guid) As Task(Of IEnumerable(Of Notification)) Implements INotificationRepository.GetByUserIdAsync
            Return Await _context.Notifications.AsNoTracking() _
                .Where(Function(n) n.UserId = userId And Not n.IsDeleted) _
                .OrderByDescending(Function(n) n.CreatedAt) _
                .ToListAsync()
        End Function

        Public Async Function AddAsync(notification As Notification) As Task Implements INotificationRepository.AddAsync
            Await _context.Notifications.AddAsync(notification)
        End Function

        Public Async Function UpdateAsync(notification As Notification) As Task Implements INotificationRepository.UpdateAsync
            notification.UpdatedAt = DateTime.UtcNow
            _context.Notifications.Update(notification)
            Await Task.CompletedTask
        End Function

        Public Async Function DeleteAsync(id As Guid) As Task Implements INotificationRepository.DeleteAsync
            Dim notification = Await GetByIdAsync(id)
            If notification IsNot Nothing Then
                notification.IsDeleted = True
                notification.UpdatedAt = DateTime.UtcNow
                _context.Notifications.Update(notification)
            End If
        End Function
    End Class

End Namespace
