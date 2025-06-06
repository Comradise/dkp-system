using System.Security.Claims;
using dkp_system_back_front.Server.Core.Models.Internal;
using dkp_system_back_front.Server.Core.Services.Implementations;
using dkp_system_back_front.Server.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dkp_system_back_front.Server.Core.Models.Internal.Guild;

namespace dkp_system_back_front.Server.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly IEventService _eventService;
    private readonly IPlayerService _playerService;

    public EventsController(ILogger<EventsController> logger, IEventService eventTypeService, IPlayerService playerService)
    {
        _logger = logger;
        _eventService = eventTypeService;
        _playerService = playerService;
    }

    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [HttpPost("submit-dkp")]
    public async Task<IActionResult> SubmitDKPCode([FromBody] SubmitDKPRequest request)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var member = await _playerService.FindByUserIdAsync(userId);
        if (member == null) return NotFound("Player not found");

        var result = await _eventService.SubmitDKPCodeAsync(member.Id, request.EventId, request.DKPCode);
        return Ok(result);
    }
}
