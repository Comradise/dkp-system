using dkp_system_back_front.Server.Core.Models.Authorization;

namespace dkp_system_back_front.Server.Core.Services.Interfaces;

public interface IAuthService
{
    public string GenerateJwtToken(ApplicationUser user);
}
