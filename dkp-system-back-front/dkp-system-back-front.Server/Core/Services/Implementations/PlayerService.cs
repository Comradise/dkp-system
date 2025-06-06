using dkp_system_back_front.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using dkp_system_back_front.Server.Core.Services.Interfaces;
using System.Security.Claims;
using dkp_system_back_front.Server.Core.Models.Internal.Guild;

public class PlayerService : IPlayerService
{
    private readonly ApplicationDbContext _context;

    public PlayerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Member?>> GetAllAsync()
    {
        return await _context.Members.ToListAsync();
    }

    public async Task<Member?> GetByIdAsync(Guid id)
    {
        return await _context.Members.FindAsync(id);
    }

    public async Task<Member> CreateAsync(string nickname)
    {
        Member player = new Member()
        {
            Nickname = nickname
        };
        _context.Members.Add(player);
        await _context.SaveChangesAsync();
        return player;
    }

    public async Task<Member?> FindByUserIdAsync(string userId)
    {
        var player = await _context.Members.FirstOrDefaultAsync(p => p.UserId.ToString() == userId);

        if (player == null)
            return null;
        return player;
    }
}
