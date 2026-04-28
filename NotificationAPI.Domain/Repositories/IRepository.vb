Imports NotificationAPI.Domain.Entities

Namespace NotificationAPI.Domain.Repositories

    Public Interface IUserRepository
        Function GetByIdAsync(id As Guid) As Task(Of User)
        Function GetAllAsync() As Task(Of IEnumerable(Of User))
        Function AddAsync(user As User) As Task
        Function UpdateAsync(user As User) As Task
        Function DeleteAsync(id As Guid) As Task
    End Interface

    Public Interface INotificationRepository
        Function GetByIdAsync(id As Guid) As Task(Of Notification)
        Function GetByUserIdAsync(userId As Guid) As Task(Of IEnumerable(Of Notification))
        Function AddAsync(notification As Notification) As Task
        Function UpdateAsync(notification As Notification) As Task
        Function DeleteAsync(id As Guid) As Task
    End Interface

    Public Interface IUnitOfWork
        ReadOnly Property Users As IUserRepository
        ReadOnly Property Notifications As INotificationRepository
        Function SaveChangesAsync() As Task(Of Integer)
        Sub Dispose()
    End Interface

End Namespace
