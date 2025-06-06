using dkp_system_back_front.Server.Core.Models;
using dkp_system_back_front.Server.Core.Models.Internal.Guild;

namespace dkp_system_back_front.Server.Core.Services.Interfaces;

public interface IPlayerService
{
    Task<IEnumerable<Member?>> GetAllAsync();
    Task<Member?> GetByIdAsync(Guid id);
    Task<Member> CreateAsync(string username);
    Task<Member?> FindByUserIdAsync(string userId);
}
