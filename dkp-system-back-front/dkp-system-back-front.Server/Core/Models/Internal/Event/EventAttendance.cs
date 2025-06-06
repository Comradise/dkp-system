using System.ComponentModel.DataAnnotations;
using dkp_system_back_front.Server.Core.Models.Internal.Guild;

namespace dkp_system_back_front.Server.Core.Models.Internal.Event;

public class EventAttendance
{
    public Guid Id { get; set; }
    [Required]
    public Guid EventId { get; set; }
    [Required]
    public Guid MemberId { get; set; }

    public Event Event { get; set; } = null!;
    public Member Member { get; set; } = null!;
}
