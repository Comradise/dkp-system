using dkp_system_back_front.Server.Core.Models.Internal.Guild;
using dkp_system_back_front.Server.Core.Services.Interfaces;
using dkp_system_back_front.Server.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace dkp_system_back_front.Server.Core.Services.Implementations;

public class GuildService : IGuildService
{
    private readonly ApplicationDbContext _context;
    public GuildService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Member> AddNewMember(Guid guildId, Guid internalUserId, string memberNickname)
    {
        Guild? guild = await _context.Guilds.FindAsync(guildId);
        if (guild == null) return null;
        if (guild.Members.Any(m => m.UserId == internalUserId)) return guild.Members.Find(m => m.UserId == internalUserId);

        Role role = _context.Roles.First(r => r.Name == "GuildMember");
        Member? member = _context.Members.FirstOrDefault(m => m.UserId == internalUserId && m.GuildId == guildId);
        if (member == null)
        {
            member = new Member()
            {
                UserId = internalUserId,
                GuildId = guildId,
                RoleId = role.Id,
                Nickname = memberNickname
            };
        }
        
        guild.Members.Add(member);
        return member;
    }

    public async Task<Member?> ChangeRole(Guid guildId, Guid memberId, Role role)
    {
        Member? member = _context.Members.FirstOrDefault(m => m.Id == memberId && m.GuildId == guildId);
        if (member == null) return null;

        member.Role = role;
        _context.SaveChanges();

        return member;
    }

    public async Task<Guild?> CreateGuild(string guildName, Guid thisUserId, string thisMemberNickname)
    {
        Guild guild = new Guild()
        {
            Name = guildName
        };
        await _context.Guilds.AddAsync(guild);
        await _context.SaveChangesAsync();

        Member member = await AddNewMember(guild.Id, thisUserId, thisMemberNickname);
        Role role = _context.Roles.First(r => r.Name == "GuildLeader");
        await ChangeRole(guild.Id, member.Id, role);
        return guild;
    }

    public async Task DeleteGuild(Guid guildId)
    {
        Guild? guild = _context.Guilds.FirstOrDefault(r => r.Id == guildId);
        if (guild == null) return;

        _context.Guilds.Remove(guild);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMember(Guid guildId, Guid internalUserId)
    {
        Guild? guild = _context.Guilds.FirstOrDefault(r => r.Id == guildId);
        if (guild == null) return;

        Member? guildMember = guild.Members.FirstOrDefault(r => r.UserId == internalUserId);
        if(guildMember == null) return;

        guild.Members.Remove(guildMember);
        await _context.SaveChangesAsync();
    }

    public Guild? FindGuild(Guid guildId)
    {
        return _context.Guilds.FirstOrDefault(g => g.Id == guildId);
    }

    public async Task<Guild?> UpdateGuild(Guild guild)
    {
        Guild? thisGuild = _context.Guilds.Find(guild);
        thisGuild = guild;
        await _context.SaveChangesAsync();

        return thisGuild;
    }
}
