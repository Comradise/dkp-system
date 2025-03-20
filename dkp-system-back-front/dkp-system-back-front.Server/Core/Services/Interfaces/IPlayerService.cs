using dkp_system_back_front.Server.Core.Models;

namespace dkp_system_back_front.Server.Core.Services.Interfaces;

public interface IPlayerService
{
    public IEnumerable<Player> GetAll();
    public Player? Get(string characterName);
    public Player Add(Player player);
    public int Delete(int id);
}
