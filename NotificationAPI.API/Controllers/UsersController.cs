using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NotificationAPI.Application.DTOs.Requests;
using NotificationAPI.Application.DTOs.Responses;
using NotificationAPI.Application.UseCases.Users;
using NotificationAPI.Domain.Entities;
using NotificationAPI.Domain.Repositories;

namespace NotificationAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly GetUserByIdUseCase _getUserByIdUseCase;

    public UsersController(IUnitOfWork unitOfWork, IMapper mapper, GetUserByIdUseCase getUserByIdUseCase)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _getUserByIdUseCase = getUserByIdUseCase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = _mapper.Map<User>(request);
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<UserResponse>(user);
            return CreatedAtAction(nameof(GetUserById), new { id = response.Id }, response);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            var response = await _getUserByIdUseCase.ExecuteAsync(id);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var responses = _mapper.Map<IEnumerable<UserResponse>>(users);
            return Ok(responses);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
    }
}
