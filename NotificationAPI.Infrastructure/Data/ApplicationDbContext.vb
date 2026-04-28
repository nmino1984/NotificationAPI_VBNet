Imports Microsoft.EntityFrameworkCore
Imports NotificationAPI.Domain.Entities

Namespace NotificationAPI.Infrastructure.Data

    Public Class ApplicationDbContext
        Inherits DbContext

        Public Property Users As DbSet(Of User)
        Public Property Notifications As DbSet(Of Notification)

        Public Sub New(options As DbContextOptions(Of ApplicationDbContext))
            MyBase.New(options)
        End Sub

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            MyBase.OnModelCreating(modelBuilder)

            ' User entity configuration
            modelBuilder.Entity(Of User)() _
                .HasKey(Function(u) u.Id)

            modelBuilder.Entity(Of User)() _
                .Property(Function(u) u.Name) _
                .IsRequired() _
                .HasMaxLength(100)

            modelBuilder.Entity(Of User)() _
                .Property(Function(u) u.Email) _
                .IsRequired() _
                .HasMaxLength(255)

            modelBuilder.Entity(Of User)() _
                .HasIndex(Function(u) u.Email) _
                .IsUnique()

            ' Notification entity configuration
            modelBuilder.Entity(Of Notification)() _
                .HasKey(Function(n) n.Id)

            modelBuilder.Entity(Of Notification)() _
                .Property(Function(n) n.Title) _
                .IsRequired() _
                .HasMaxLength(200)

            modelBuilder.Entity(Of Notification)() _
                .Property(Function(n) n.Message) _
                .IsRequired() _
                .HasMaxLength(1000)

            ' One-to-Many: User -> Notifications
            modelBuilder.Entity(Of Notification)() _
                .HasOne(Function(n) n.User) _
                .WithMany() _
                .HasForeignKey(Function(n) n.UserId)
        End Sub
    End Class

End Namespace
