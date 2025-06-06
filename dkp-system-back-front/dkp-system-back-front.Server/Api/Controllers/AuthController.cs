using dkp_system_back_front.Server.Core.Models.Authorization;
using dkp_system_back_front.Server.Core.Models.Internal;
using dkp_system_back_front.Server.Core.Services.Interfaces;
using dkp_system_back_front.Server.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dkp_system_back_front.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;
    private readonly ApplicationDbContext _context;
    private readonly IAuthService _authService;

    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config, ApplicationDbContext context, IAuthService authService)
    {
        _userManager = userManager;
        _config = config;
        _context = context;
        _authService = authService;
    }

    /*[HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        var player = new Player { UserId = user.Id, Username = request.Nickname };
        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        return Ok("Registration successful");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            return Unauthorized();

        var token = _authService.GenerateJwtToken(user);
        return Ok(new { Token = token });
    }*/

    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [HttpGet("me")]
    public async Task<ApplicationUser> GetInfo()
    {
        string userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        return await _context.Users.FindAsync(userId);
    }
}
