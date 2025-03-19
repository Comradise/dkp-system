using dkp_system_back_front.Server.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace dkp_system_back_front.Server.Infrastructure.Data;

public partial class ApplicationDbContext : DbContext
{
    public DbSet<EventType> EventTypes { get; set; }
    public DbSet<Player> Players { get; set; }
}
