using System.ComponentModel.DataAnnotations;

namespace dkp_system_back_front.Server.Core.Models.Internal.Guild;

public class SubmitDKPRequest
{
    [Required]
    public Guid EventId { get; set; } = Guid.NewGuid();
    [Required]
    public string DKPCode { get; set; } = string.Empty;
}
