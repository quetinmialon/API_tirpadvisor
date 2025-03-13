using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tripAdvisorAPI.DTO;
using tripAdvisorAPI.Services;

namespace tripAdvisorAPI.Controllers;

[Route("api/cauth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    

    [HttpPost("register")]
    public async Task<ActionResult<string>> CreaterUser([FromBody] CreateUserDTO registerUserDTO)
    {
        await _authService.CreateUser(registerUserDTO);
        return Ok();
    }
}
