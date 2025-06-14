using dkp_system_back_front.Server.Core.Models.Internal.Guild;

namespace dkp_system_back_front.Server.Core.Services.Interfaces;

public interface IGuildService
{
    public Task<Member?> AddNewMember(Guid guildId, string internalUserId, string memberNickname);
    public Task<Guild?> CreateGuild(string guildName, string internalUserId, string thisMemberNickname);
    public Task DeleteGuild(Guid guildId);
    public Task DeleteMember(Guid guildId, string internalUserId);
    public Guild? FindGuild(Guid guildId);
    public Task<Guild?> UpdateGuild(Guild guild);
    public Task<Member?> ChangeRole(Guid guildId, string internalUserId, Role role);
}
