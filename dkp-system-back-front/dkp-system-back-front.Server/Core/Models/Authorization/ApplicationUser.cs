using dkp_system_back_front.Server.Core.Models.Internal.Guild;
using Microsoft.AspNetCore.Identity;

namespace dkp_system_back_front.Server.Core.Models.Authorization;

public class ApplicationUser : IdentityUser
{
    public ICollection<Member> Members { get; set; } = new List<Member>();
}
