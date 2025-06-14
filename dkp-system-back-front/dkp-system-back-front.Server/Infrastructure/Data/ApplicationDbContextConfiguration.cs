using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using dkp_system_back_front.Server.Core.Models.Internal.Event;
using dkp_system_back_front.Server.Core.Models.Internal.Guild;
using dkp_system_back_front.Server.Core.Models.Internal;

namespace dkp_system_back_front.Server.Infrastructure.Data;

public partial class ApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Guild - Member (1 - many)
        modelBuilder.Entity<Guild>()
            .HasMany(g => g.Members)
            .WithOne(m => m.Guild)
            .HasForeignKey(m => m.GuildId)
            .OnDelete(DeleteBehavior.Cascade);

        // Guild - Event (1 - many)
        modelBuilder.Entity<Guild>()
            .HasMany(g => g.Events)
            .WithOne(e => e.Guild)
            .HasForeignKey(e => e.GuildId)
            .OnDelete(DeleteBehavior.Cascade);

        // Member - EventAttendance (1 - many)
        modelBuilder.Entity<Member>()
            .HasMany(m => m.Attendances)
            .WithOne(a => a.Member)
            .HasForeignKey(a => a.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        // Member - Role (many - 1)
        modelBuilder.Entity<Member>()
            .HasOne(m => m.Role)
            .WithMany(r => r.Members)
            .HasForeignKey(m => m.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Member - User (many - 1)
        modelBuilder.Entity<Member>()
            .HasOne(m => m.User)
            .WithMany(r => r.Members)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Event - EventType (many - 1)
        modelBuilder.Entity<Event>()
            .HasOne(e => e.EventType)
            .WithMany(t => t.Events)
            .HasForeignKey(e => e.EventTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Event - EventAttendance (1 - many)
        modelBuilder.Entity<Event>()
            .HasMany(e => e.Attendances)
            .WithOne(a => a.Event)
            .HasForeignKey(a => a.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // Event - Rewards (1 - many)
        modelBuilder.Entity<Event>()
            .HasMany(e => e.Rewards)
            .WithOne(r => r.Event)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // Уникальность по двум полям (MemberId, EventId)
        modelBuilder.Entity<EventAttendance>()
            .HasIndex(a => new { a.MemberId, a.EventId })
            .IsUnique();

        // Ограничение на длину кодов
        modelBuilder.Entity<Event>()
            .Property(e => e.DkpCode)
            .HasMaxLength(4);

        // Ограничение на длину названия
        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .HasMaxLength(64)
            .IsRequired();

        // Ограничение на длину названия
        modelBuilder.Entity<EventType>()
            .Property(e => e.Name)
            .HasMaxLength(64)
            .IsRequired();

        // Ограничение на длину имени
        modelBuilder.Entity<Member>()
            .Property(m => m.Nickname)
            .HasMaxLength(64);


        // Добавление ролей
        modelBuilder.Entity<Role>().HasData(
            new Role() { Id = 1, Name = "GuildLeader" },
            new Role() { Id = 2, Name = "GuildMember" }
        );
    }
}
