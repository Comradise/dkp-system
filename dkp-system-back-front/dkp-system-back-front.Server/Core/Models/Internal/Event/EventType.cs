using System.ComponentModel.DataAnnotations;

namespace dkp_system_back_front.Server.Core.Models.Internal;

public class EventType
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;

    public List<Event.Event> Events { get; set; } = new();
}
