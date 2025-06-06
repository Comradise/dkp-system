using Microsoft.AspNetCore.Mvc;
using dkp_system_back_front.Server.Core.Models.Internal;
using dkp_system_back_front.Server.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace dkp_system_back_front.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayersController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _playerService.GetAllAsync());

    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var player = await _playerService.GetByIdAsync(id);
        return player == null ? NotFound() : Ok(player);
    }

    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create(string username)
    {
        var created = await _playerService.CreateAsync(username);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}