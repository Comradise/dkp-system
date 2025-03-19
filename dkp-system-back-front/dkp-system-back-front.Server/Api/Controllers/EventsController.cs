using dkp_system_back_front.Server.Core.Models;
using dkp_system_back_front.Server.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace dkp_system_back_front.Server.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly IEventTypeService _eventTypeService;

    public EventsController(ILogger<EventsController> logger, IEventTypeService eventTypeService)
    {
        _logger = logger;
        _eventTypeService = eventTypeService;
    }

    [HttpGet]
    [Route("GetAll")]
    public IEnumerable<EventType> GetAll()
    {
        return _eventTypeService.GetAll();
    }

    [HttpGet]
    [Route("Get")]
    public EventType GetEvent(int id)
    {
        return _eventTypeService.GetEvent(id);
    }

    [HttpPost]
    [Route("Add")]
    public EventType Add(EventType eventType)
    {
        return _eventTypeService.Add(eventType);
    }
}
