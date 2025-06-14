namespace dkp_system_back_front.Server.Core.Models.Internal.Guild;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Member> Members { get; set; } = new();
}
