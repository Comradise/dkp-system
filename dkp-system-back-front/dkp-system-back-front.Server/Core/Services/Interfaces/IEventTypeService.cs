using dkp_system_back_front.Server.Core.Models;

namespace dkp_system_back_front.Server.Core.Services.Interfaces;

public interface IEventTypeService
{
    public IEnumerable<EventType> GetAll();
    public EventType? GetEvent(int id);
    public EventType Add(EventType eventType);
    public int Delete(EventType eventType);
    public EventType Update(EventType eventType);
}
