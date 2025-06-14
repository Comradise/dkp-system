using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using dkp_system_back_front.Server.Core.Models.Internal.Guild;
using dkp_system_back_front.Server.Core.Services.Implementations;
using dkp_system_back_front.Server.Infrastructure.Data;

namespace dkp_system_back_front.Server.Test;

[TestFixture]
public class GuildServiceTests
{
    private ApplicationDbContext _context;
    private GuildService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _service = new GuildService(_context);

        // Создать роли
        var roles = new List<Role>
        {
            new Role()
            {
                Id = 1,
                Name = "GuildLeader"
            },
            new Role()
            {
                Id = 2,
                Name = "GuildMember"
            }
        };
        _context.Roles.AddRange(roles);
        _context.SaveChanges();
    }

    [Test]
    public async Task CreateGuild_ShouldCreateGuildAndAssignLeader()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var nickname = "TestLeader";

        // Act
        var guild = await _service.CreateGuild("TestGuild", userId, nickname);

        // Assert
        Assert.IsNotNull(guild);
        Assert.That(guild.Name, Is.EqualTo("TestGuild"));

        var member = _context.Members.FirstOrDefault(m => m.UserId == userId);
        Assert.IsNotNull(member);
        Assert.That(member.Nickname, Is.EqualTo(nickname));

        Assert.That(member.RoleId, Is.EqualTo(1));
    }

    [Test]
    public async Task AddNewMember_ShouldAddMemberIfNotExists()
    {
        var guild = new Guild { Id = Guid.NewGuid(), Name = "Guild1" };
        _context.Guilds.Add(guild);
        _context.SaveChanges();

        var userId = "test";

        var result = await _service.AddNewMember(guild.Id, userId, "Member1");

        Assert.IsNotNull(result);
        Assert.That(result.UserId, Is.EqualTo(userId));
    }

    [Test]
    public async Task DeleteGuild_ShouldRemoveGuild()
    {
        var guild = new Guild { Id = Guid.NewGuid(), Name = "ToDelete" };
        _context.Guilds.Add(guild);
        _context.SaveChanges();

        await _service.DeleteGuild(guild.Id);

        var exists = _context.Guilds.Any(g => g.Id == guild.Id);
        Assert.IsFalse(exists);
    }

    [Test]
    public async Task DeleteMember_ShouldRemoveMemberFromGuild()
    {
        var userId = "test";
        var guild = new Guild { Id = Guid.NewGuid(), Name = "GuildWithMember" };
        var member = new Member { UserId = userId, GuildId = guild.Id };
        guild.Members.Add(member);
        _context.Guilds.Add(guild);
        _context.SaveChanges();

        await _service.DeleteMember(guild.Id, userId);

        var updatedGuild = _context.Guilds.Include(g => g.Members).First(g => g.Id == guild.Id);
        Assert.IsEmpty(updatedGuild.Members);
    }

    [Test]
    public void FindGuild_ShouldReturnGuild()
    {
        var guild = new Guild { Id = Guid.NewGuid(), Name = "FindMe" };
        _context.Guilds.Add(guild);
        _context.SaveChanges();

        var result = _service.FindGuild(guild.Id);
        Assert.IsNotNull(result);
        Assert.That(result.Name, Is.EqualTo("FindMe"));
    }
}
