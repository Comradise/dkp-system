using System.Security.Claims;
using dkp_system_back_front.Server.Core.Models.Internal.Guild;
using dkp_system_back_front.Server.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dkp_system_back_front.Server.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class GuildController : ControllerBase
{
    private readonly ILogger<GuildController> _logger;
    private readonly IGuildService _guildService;
    public GuildController(ILogger<GuildController> logger, IGuildService guildService)
    {
        _logger = logger;
        _guildService = guildService;
    }

    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [HttpPost("create")]
    public async Task<Guild?> Create(string guildName, string thisMemberNickname)
    {
        string userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        return await _guildService.CreateGuild(guildName, userId, thisMemberNickname);
    }
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [HttpPost("delete")]
    public async Task Delete(Guid guildId)
    {
        string userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        Role? role = _guildService.GetRole(guildId, userId);
        if (role == null) return;
        if (role.Id != 1) return;

        await _guildService.DeleteGuild(guildId);
    }
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [HttpPost("update")]
    public async Task<Guild?> Update(Guild guild)
    {
        string userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        Role? role = _guildService.GetRole(guild.Id, userId);
        if (role == null) return null;
        if (role.Id != 1) return null;

        return await _guildService.UpdateGuild(guild);
    }
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [HttpPost("delete-member")]
    public async void DeleteMember(Guid guildId)
    {
        string userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        Role? role = _guildService.GetRole(guildId, userId);
        if (role == null) return;
        if (role.Id != 1) return;

        await _guildService.DeleteMember(guildId, userId);
    }
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [HttpPost("add-new-member")]
    public async Task<Member?> AddNewMember(Guid guildId, string memberNickname)
    {
        string userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        Role? role = _guildService.GetRole(guildId, userId);
        if (role == null) return null;
        if (role.Id != 1) return null;

        return await _guildService.AddNewMember(guildId, userId, memberNickname);
    }
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [HttpPost("change-role")]
    public async Task<Member?> ChangeRole(Guid guildId, Role role)
    {
        string userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        Role? userRole = _guildService.GetRole(guildId, userId);
        if (userRole == null) return null;
        if (userRole.Id != 1) return null;

        return await _guildService.ChangeRole(guildId, userId, role);
    }
}
