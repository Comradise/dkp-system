namespace dkp_system_back_front.Server.Core.Models.Authorization;

public record LoginRequest
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
