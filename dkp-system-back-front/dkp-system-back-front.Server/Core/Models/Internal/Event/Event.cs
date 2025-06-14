using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace dkp_system_back_front.Server.Core.Models.Internal.Event;
public class Event
{
    public Guid Id { get; set; }
    [Required]
    public Guid EventTypeId { get; set; }
    public string DkpCode { get; set; } = "0000";
    [Required]
    public int Reward { get; set; }
    public DateTime BeginDT { get; set; } = DateTime.UtcNow;
    public TimeSpan ExpirationTime { get; set; } = TimeSpan.FromHours(1);
    [Required]
    public Guid GuildId { get; set; }

    public EventType EventType { get; set; } = null!;
    public Guild.Guild Guild { get; set; }
    public List<EventAttendance> Attendances { get; set; } = new();
    public List<Rewards> Rewards { get; set; } = new();
}

