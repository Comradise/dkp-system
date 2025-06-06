using System.ComponentModel.DataAnnotations;

namespace dkp_system_back_front.Server.Core.Models.Internal.Event;

public class Rewards
{
    public Guid Id { get; set; }
    [Required]
    public Guid EventId { get; set; }
    public int DkpAmount { get; set; }

    public Event Event { get; set; } = null!;
}
