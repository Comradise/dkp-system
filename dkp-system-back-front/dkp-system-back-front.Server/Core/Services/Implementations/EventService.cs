using dkp_system_back_front.Server.Core.Models.Internal;
using dkp_system_back_front.Server.Core.Models.Internal.Event;
using dkp_system_back_front.Server.Core.Models.Internal.Guild;
using dkp_system_back_front.Server.Core.Services.Interfaces;
using dkp_system_back_front.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace dkp_system_back_front.Server.Core.Services.Implementations;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;

    public EventService(ApplicationDbContext db)
    {
        _context = db;
    }

    public async Task<string> SubmitDKPCodeAsync(Guid memberId, Guid eventId, string dkpCode)
    {
        Member? player = await _context.Members.Include(p => p.Attendances).FirstOrDefaultAsync(p => p.Id == memberId);
        if (player == null) return "Player not found";

        Event? thisEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
        if (thisEvent == null) return "Invalid event";
        if (thisEvent.DkpCode != dkpCode) return "Invalid DKP code";

        if (thisEvent.BeginDT + thisEvent.ExpirationTime < DateTime.UtcNow)
            return "DKP code has expired";

        bool alreadySubmitted = await _context.EventAttendances
            .AnyAsync(a => a.MemberId == memberId && a.EventId == thisEvent.Id);
        if (alreadySubmitted) return "DKP code already used";

        EventAttendance attendance = new EventAttendance
        {
            MemberId = memberId,
            EventId = thisEvent.Id
        };

        _context.EventAttendances.Add(attendance);
        player.Dkp += thisEvent.Reward;

        await _context.SaveChangesAsync();
        return $"Success! +{thisEvent.Reward} DKP";
    }
}
