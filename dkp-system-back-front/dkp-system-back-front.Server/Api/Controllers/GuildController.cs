using dkp_system_back_front.Server.Core.Models.Internal.Guild;
using dkp_system_back_front.Server.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace dkp_system_back_front.Server.Api.Controllers;
public class GuildController : ControllerBase
{
    private readonly ILogger<GuildController> _logger;
    private readonly IGuildService _guildService;
    public GuildController(ILogger<GuildController> logger, IGuildService guildService)
    {
        _logger = logger;
        _guildService = guildService;
    }
    public async Task<Guild?> Create(string guildName, Guid thisMemberId, string thisMemberNickname)
    {
        return await _guildService.CreateGuild(guildName, thisMemberId, thisMemberNickname);
    }

    public async Task Delete(Guid guildId)
    {
        await _guildService.DeleteGuild(guildId);
    }

    public async Task<Guild?> Update(Guild guild)
    {
        return await _guildService.UpdateGuild(guild);
    }

    public async void DeleteMember(Guid guildId, Guid memberId)
    {
        await _guildService.DeleteMember(guildId, memberId);
    }

    public async Task<Member?> AddNewMember(Guid guildId, Guid internalUserId, string memberNickname)
    {
        return await _guildService.AddNewMember(guildId, internalUserId, memberNickname);
    }

    public async Task<Member?> ChangeRole(Guid guildId, Guid memberId, Role role)
    {
        return await _guildService.ChangeRole(guildId, memberId, role);
    }
}
