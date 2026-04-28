Imports AutoMapper
Imports NotificationAPI.Application.DTOs.Requests
Imports NotificationAPI.Application.DTOs.Responses
Imports NotificationAPI.Domain.Entities

Namespace NotificationAPI.Application.Mappings

    Public Class MappingProfile
        Inherits Profile

        Public Sub New()
            ' User mappings
            CreateMap(Of CreateUserRequest, User) _
                .ForMember(Function(dest) dest.Id, Sub(cfg) cfg.Ignore()) _
                .ForMember(Function(dest) dest.CreatedAt, Sub(cfg) cfg.Ignore()) _
                .ForMember(Function(dest) dest.UpdatedAt, Sub(cfg) cfg.Ignore()) _
                .ForMember(Function(dest) dest.IsDeleted, Sub(cfg) cfg.Ignore())

            CreateMap(Of User, UserResponse)

            ' Notification mappings
            CreateMap(Of SendNotificationRequest, Notification) _
                .ForMember(Function(dest) dest.Id, Sub(cfg) cfg.Ignore()) _
                .ForMember(Function(dest) dest.CreatedAt, Sub(cfg) cfg.Ignore()) _
                .ForMember(Function(dest) dest.UpdatedAt, Sub(cfg) cfg.Ignore()) _
                .ForMember(Function(dest) dest.IsDeleted, Sub(cfg) cfg.Ignore())

            CreateMap(Of Notification, NotificationResponse)
        End Sub
    End Class

End Namespace
