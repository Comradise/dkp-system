using dkp_system_back_front.Server.Core.Models;
using dkp_system_back_front.Server.Core.Services.Interfaces;
using dkp_system_back_front.Server.Infrastructure.Data;

namespace dkp_system_back_front.Server.Core.Services.Implementations;

public class PlayerService : IPlayerService
{
    private readonly ApplicationDbContext _db;

    public PlayerService(ApplicationDbContext db)
    {
        _db = db;
    }

    public Player Add(Player player)
    {
        _db.Players.Add(player);
        _db.SaveChanges();

        return player;
    }

    public int Delete(Player player)
    {
        _db.Players.Remove(player);
        _db.SaveChanges();

        return player.Id;
    }

    public IEnumerable<Player> GetAll()
    {
        return _db.Players.ToList();
    }

    public Player? Get(string characterName)
    {
        return _db.Players.FirstOrDefault(e => e.CharacterName == characterName);
    }
}
