using System.ComponentModel.DataAnnotations;
using dkp_system_back_front.Server.Core.Models.Authorization;
using dkp_system_back_front.Server.Core.Models.Internal.Event;
using dkp_system_back_front.Server.Core.Models.Internal.User;

namespace dkp_system_back_front.Server.Core.Models.Internal.Guild;

public class Member
{
    public Guid Id { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid GuildId { get; set; }
    [Required]
    public Guid RoleId { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public int Dkp { get; set; } = 0;

    public InternalUser User { get; set; } = null!;
    public Guild Guild { get; set; } = null!;
    public Role Role { get; set; } = null!;
    public List<EventAttendance> Attendances { get; set; } = new();
}
