using dkp_system_back_front.Server.Core.Models.Internal;

namespace dkp_system_back_front.Server.Core.Services.Interfaces;

public interface IEventService
{
    public Task<string> SubmitDKPCodeAsync(Guid playerId, Guid eventId, string dkpCode);
}
