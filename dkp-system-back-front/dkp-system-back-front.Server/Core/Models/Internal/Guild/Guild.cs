using dkp_system_back_front.Server.Core.Models.Internal.Event;

namespace dkp_system_back_front.Server.Core.Models.Internal.Guild;

public class Guild
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Member> Members { get; set; } = new();
    public List<Event.Event> Events { get; set; } = new();
}
