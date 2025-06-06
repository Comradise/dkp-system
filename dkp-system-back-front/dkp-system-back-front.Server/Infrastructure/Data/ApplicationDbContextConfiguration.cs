using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using dkp_system_back_front.Server.Core.Models.Internal.Event;
using dkp_system_back_front.Server.Core.Models.Internal.Guild;

namespace dkp_system_back_front.Server.Infrastructure.Data;

public partial class ApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        /*modelBuilder.Entity<Member>()
        .HasMany(p => p.Attendances)
        .WithOne(a => a.Player)
        .HasForeignKey(a => a.PlayerId);

        modelBuilder.Entity<Event>()
            .HasMany(e => e.Attendances)
            .WithOne(a => a.Event)
            .HasForeignKey(a => a.EventId);

        modelBuilder.Entity<EventAttendance>()
            .HasIndex(a => new { a.PlayerId, a.EventId })
            .IsUnique();*/

        modelBuilder.HasDefaultSchema("identity");
    }
}
