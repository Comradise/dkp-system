using dkp_system_back_front.Server.Core.Models.Internal.Event;
using dkp_system_back_front.Server.Core.Models.Internal.Guild;
using dkp_system_back_front.Server.Core.Services.Implementations;
using dkp_system_back_front.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace dkp_system_back_front.Server.Test;

public class Tests
{
    private ApplicationDbContext _context;
    private EventService _eventService;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
        _eventService = new EventService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task SubmitDKPCodeAsync_PlayerNotFound_ReturnsError()
    {
        var result = await _eventService.SubmitDKPCodeAsync(Guid.NewGuid(), Guid.NewGuid(), "code");
        Assert.AreEqual("Player not found", result);
    }

    [Test]
    public async Task SubmitDKPCodeAsync_EventNotFound_ReturnsError()
    {
        var member = new Member { Id = Guid.NewGuid() };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var result = await _eventService.SubmitDKPCodeAsync(member.Id, Guid.NewGuid(), "code");
        Assert.AreEqual("Invalid event", result);
    }

    [Test]
    public async Task SubmitDKPCodeAsync_InvalidCode_ReturnsError()
    {
        var member = new Member { Id = Guid.NewGuid() };
        var evt = new Event
        {
            Id = Guid.NewGuid(),
            DkpCode = "correct-code",
            BeginDT = DateTime.UtcNow,
            ExpirationTime = TimeSpan.FromHours(1),
            Reward = 10
        };
        _context.Members.Add(member);
        _context.Events.Add(evt);
        await _context.SaveChangesAsync();

        var result = await _eventService.SubmitDKPCodeAsync(member.Id, evt.Id, "wrong-code");
        Assert.AreEqual("Invalid DKP code", result);
    }

    [Test]
    public async Task SubmitDKPCodeAsync_CodeExpired_ReturnsError()
    {
        var member = new Member { Id = Guid.NewGuid() };
        var evt = new Event
        {
            Id = Guid.NewGuid(),
            DkpCode = "code",
            BeginDT = DateTime.UtcNow.AddHours(-2),
            ExpirationTime = TimeSpan.FromHours(1),
            Reward = 10
        };
        _context.Members.Add(member);
        _context.Events.Add(evt);
        await _context.SaveChangesAsync();

        var result = await _eventService.SubmitDKPCodeAsync(member.Id, evt.Id, "code");
        Assert.AreEqual("DKP code has expired", result);
    }

    [Test]
    public async Task SubmitDKPCodeAsync_AlreadySubmitted_ReturnsError()
    {
        var memberId = Guid.NewGuid();
        var eventId = Guid.NewGuid();

        var member = new Member { Id = memberId };
        var evt = new Event
        {
            Id = eventId,
            DkpCode = "code",
            BeginDT = DateTime.UtcNow,
            ExpirationTime = TimeSpan.FromHours(1),
            Reward = 10
        };
        var attendance = new EventAttendance { MemberId = memberId, EventId = eventId };

        _context.Members.Add(member);
        _context.Events.Add(evt);
        _context.EventAttendances.Add(attendance);
        await _context.SaveChangesAsync();

        var result = await _eventService.SubmitDKPCodeAsync(memberId, eventId, "code");
        Assert.AreEqual("DKP code already used", result);
    }

    [Test]
    public async Task SubmitDKPCodeAsync_ValidSubmission_ReturnsSuccess()
    {
        var memberId = Guid.NewGuid();
        var eventId = Guid.NewGuid();

        var member = new Member { Id = memberId, Dkp = 0 };
        var evt = new Event
        {
            Id = eventId,
            DkpCode = "code",
            BeginDT = DateTime.UtcNow,
            ExpirationTime = TimeSpan.FromHours(1),
            Reward = 25
        };

        _context.Members.Add(member);
        _context.Events.Add(evt);
        await _context.SaveChangesAsync();

        var result = await _eventService.SubmitDKPCodeAsync(memberId, eventId, "code");

        Assert.AreEqual("Success! +25 DKP", result);
        var attendance = await _context.EventAttendances.FirstOrDefaultAsync(a => a.MemberId == memberId && a.EventId == eventId);
        Assert.IsNotNull(attendance);

        var updatedMember = await _context.Members.FindAsync(memberId);
        Assert.AreEqual(25, updatedMember.Dkp);
    }
}