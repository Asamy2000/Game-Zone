using GameZone.Data;
using GameZone.Services;
using GameZone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesServices _devicesServices;
        private readonly IGamesService _gamesService;

        public GamesController(/*ApplicationDbContext context,*/ ICategoriesService categoriesService, IDevicesServices devicesServices, IGamesService gamesService)
        {
            //_context = context;
            _categoriesService = categoriesService;
            _devicesServices = devicesServices;
            _gamesService = gamesService;
        }

        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }

        public IActionResult Details(int Id)
        {
            var game = _gamesService.GetById(Id);
              if(game == null)
                return NotFound();

            return View(game);
        }


        public IActionResult Create()
        {
            CreateGameViewModel viewModel = new()
            {
                //projection
                Categories = _categoriesService.GetCategories(),

                 Devices = _devicesServices.GetDevices()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  Create(CreateGameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetCategories();
                model.Devices = _devicesServices.GetDevices();
                return View(model);
            }
            await _gamesService.createAsync(model);
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Edit(int id)
        {
            var game = _gamesService.GetById(id);

            if (game is null)
                return NotFound();

            EditGameViewModel viewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
                Categories = _categoriesService.GetCategories(),
                Devices = _devicesServices.GetDevices(),
                CurrentCover = game.Cover
            };

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetCategories();
                model.Devices = _devicesServices.GetDevices();
                return View(model);
            }
             var game = await _gamesService.Update(model);
            if (game is null)
                return BadRequest();
            return RedirectToAction(nameof(Index));
        }



        //[HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gamesService.Delete(id);

            return isDeleted ? Ok() : BadRequest();
        }
    }
}
