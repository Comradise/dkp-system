using dkp_system_back_front.Server.Core.Models;
using dkp_system_back_front.Server.Core.Services.Interfaces;
using dkp_system_back_front.Server.Infrastructure.Data;

namespace dkp_system_back_front.Server.Core.Services.Implementations;

public class EventTypeService : IEventTypeService
{
    private readonly ApplicationDbContext _db;

    public EventTypeService(ApplicationDbContext db)
    {
        _db = db;
    }

    public EventType Add(EventType eventType)
    {
        _db.EventTypes.Add(eventType);
        _db.SaveChanges();

        return eventType;
    }

    public int Delete(EventType eventType)
    {
        _db.EventTypes.Remove(eventType);
        _db.SaveChanges();

        return eventType.Id;
    }

    public IEnumerable<EventType> GetAll()
    {
        return _db.EventTypes.ToList();
    }

    public EventType? GetEvent(int id)
    {
        return _db.EventTypes.FirstOrDefault(e => e.Id == id);
    }

    public EventType Update(EventType eventType)
    {
        EventType oldEventType = _db.EventTypes.First(e => e.Id == eventType.Id);
        oldEventType = eventType;
        _db.SaveChanges();

        return eventType;
    }
}
