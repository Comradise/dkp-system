using dkp_system_back_front.Server.Core.Models;
using dkp_system_back_front.Server.Core.Services.Implementations;
using dkp_system_back_front.Server.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dkp_system_back_front.Server.Api.Controllers;
[Route("[controller]")]
[ApiController]
public class PlayersController : ControllerBase
{
    private readonly ILogger<PlayersController> _logger;
    private readonly IPlayerService _playerService;

    public PlayersController(ILogger<PlayersController> logger, IPlayerService playerService)
    {
        _logger = logger;
        _playerService = playerService;
    }

    [HttpGet]
    [Route("GetAll")]
    public IEnumerable<Player> GetAll()
    {
        return _playerService.GetAll();
    }

    [HttpGet]
    [Route("Get")]
    public Player? Get(string characterName)
    {
        return _playerService.Get(characterName);
    }

    [HttpPost]
    [Route("Add")]
    public Player Add(Player player)
    {
        return _playerService.Add(player);
    }

    [HttpDelete]
    [Route("Delete")]
    public int Delete(int id)
    {
        return _playerService.Delete(id);
    }
}
