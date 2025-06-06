using dkp_system_back_front.Server.Core.Models;
using dkp_system_back_front.Server.Core.Models.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using dkp_system_back_front.Server.Core.Models.Authorization;
using dkp_system_back_front.Server.Core.Models.Internal.Event;
using dkp_system_back_front.Server.Core.Models.Internal.Guild;
using System;

namespace dkp_system_back_front.Server.Infrastructure.Data;

public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Guild> Guilds => Set<Guild>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<EventType> EventTypes => Set<EventType>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventAttendance> EventAttendances => Set<EventAttendance>();
    public DbSet<Rewards> Rewards => Set<Rewards>();
}
