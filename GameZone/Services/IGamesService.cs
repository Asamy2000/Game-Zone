using GameZone.Models;
using GameZone.ViewModels;

namespace GameZone.Services
{
    public interface IGamesService
    {
        Task createAsync(CreateGameViewModel viewModel);
        IEnumerable<Game> GetAll();
        Game? GetById(int gameId);

        Task<Game?> Update(EditGameViewModel viewModel);

        bool Delete(int gameId);
    }
}


