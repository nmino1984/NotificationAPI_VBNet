using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NotificationAPI.Application.DTOs.Requests;
using NotificationAPI.Application.DTOs.Responses;
using NotificationAPI.Application.UseCases.Notifications;
using NotificationAPI.Domain.Repositories;

namespace NotificationAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly SendNotificationUseCase _sendNotificationUseCase;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<SendNotificationRequest> _validator;

    public NotificationsController(
        SendNotificationUseCase sendNotificationUseCase,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<SendNotificationRequest> validator)
    {
        _sendNotificationUseCase = sendNotificationUseCase;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpPost("send")]
    [ProducesResponseType(typeof(NotificationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { error = e.ErrorMessage }));

        try
        {
            var response = await _sendNotificationUseCase.ExecuteAsync(request);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNotificationById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(NotificationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotificationById(Guid id)
    {
        try
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            if (notification == null)
                return NotFound(new { error = $"Notification with ID {id} not found" });

            var response = _mapper.Map<NotificationResponse>(notification);
            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
    }

    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<NotificationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotificationsByUserId(Guid userId)
    {
        try
        {
            var notifications = await _unitOfWork.Notifications.GetByUserIdAsync(userId);
            var responses = _mapper.Map<IEnumerable<NotificationResponse>>(notifications);
            return Ok(responses);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
    }
}
