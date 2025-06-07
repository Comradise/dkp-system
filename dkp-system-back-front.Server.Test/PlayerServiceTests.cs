using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using dkp_system_back_front.Server.Core.Models.Internal.Guild;
using dkp_system_back_front.Server.Infrastructure.Data;

namespace dkp_system_back_front.Server.Test;


[TestFixture]
public class PlayerServiceTests
{
    private ApplicationDbContext _context;
    private PlayerService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _service = new PlayerService(_context);
    }

    [Test]
    public async Task CreateAsync_ShouldAddNewMember()
    {
        var result = await _service.CreateAsync("NewPlayer");
        Assert.IsNotNull(result);
        Assert.That(result.Nickname, Is.EqualTo("NewPlayer"));
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllMembers()
    {
        _context.Members.AddRange(
            new Member { Nickname = "P1" },
            new Member { Nickname = "P2" }
        );
        _context.SaveChanges();

        var result = await _service.GetAllAsync();
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnCorrectMember()
    {
        var member = new Member { Id = Guid.NewGuid(), Nickname = "P3" };
        _context.Members.Add(member);
        _context.SaveChanges();

        var result = await _service.GetByIdAsync(member.Id);
        Assert.IsNotNull(result);
        Assert.That(result.Nickname, Is.EqualTo("P3"));
    }

    [Test]
    public async Task FindByUserIdAsync_ShouldReturnMember()
    {
        var userId = Guid.NewGuid();
        _context.Members.Add(new Member { UserId = userId, Nickname = "ById" });
        _context.SaveChanges();

        var result = await _service.FindByUserIdAsync(userId.ToString());
        Assert.IsNotNull(result);
        Assert.That(result.Nickname, Is.EqualTo("ById"));
    }
}

