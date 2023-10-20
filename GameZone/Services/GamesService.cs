using GameZone.Data;
using GameZone.Models;
using GameZone.Settings;
using GameZone.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GameZone.Services
{
    public class GamesService : IGamesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagePath;

        public GamesService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            //  wwwroot/assets/images/games
            _imagePath = $"{_webHostEnvironment.WebRootPath}{FileSettings.imgesPath}";
        }
        public async Task createAsync(CreateGameViewModel viewModel)
        {
            var coverName = await SaveCoverAsync(viewModel.Cover);


            Game game = new()
            {
               Name = viewModel.Name,
               Cover = coverName,
               CategoryId = viewModel.CategoryId,
               Description = viewModel.Description,
               Devices = viewModel.SelectedDevices.Select(d => new GameDevice { DeviceId = d}).ToList(),
            };
            await _context.AddAsync(game);
            await _context.SaveChangesAsync();
           
        }

    

        public IEnumerable<Game> GetAll()
        {
            //Eager loading
            return _context.Games.Include(G => G.Category)
                .Include(G => G.Devices)
                .ThenInclude(D => D.Device)
                .AsNoTracking().ToList();
        }

        public Game? GetById(int gameId)
        {
            return _context.Games.Include(G => G.Category)
             .Include(G => G.Devices)
             .ThenInclude(D => D.Device)
             .AsNoTracking()
             .SingleOrDefault(g => g.Id == gameId);
        }

        public async Task<Game?> Update(EditGameViewModel viewModel)
        {
            // first get the game from the database - check if null
            var game = _context.Games
                .Include(g => g.Devices)
                .SingleOrDefault(g => g.Id == viewModel.Id);
            if (game is null)
                return null;

            var hasNewCover = viewModel.Cover is not null; //user added a new cover
            var oldCover = game.Cover; //if user not added a new cover
            //manual mapping
            game.Name = viewModel.Name;
            game.Description = viewModel.Description;
            game.CategoryId = viewModel.CategoryId;
            //projection [change the datatype from list<int> to list<GameDevice>]
            game.Devices = viewModel.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();

            if(hasNewCover)
                game.Cover = await SaveCoverAsync(viewModel.Cover!);

            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                if (hasNewCover)
                {
                    // delete the old cover which is exist on the server
                    var cover = Path.Combine(_imagePath, oldCover);
                    File.Delete(cover);
                }

                return game;
            }
            else
            {
                //delete the added cover on the server
                var cover = Path.Combine(_imagePath, game.Cover);
                File.Delete(cover);

                return null;
            }
        }


        public bool Delete(int gameId)
        {
            var isDeleted = false;

            var game = _context.Games.Find(gameId);

             if(game is null)
               return isDeleted;

             _context.Remove(game);
            var effectedRows = _context.SaveChanges();
            if (effectedRows > 0)
            {
                isDeleted = true;
                //delete the image which is saved befor on the server
                var cover = Path.Combine(_imagePath, game.Cover);
                File.Delete(cover);
                
            }
            return isDeleted;

        }

        private async Task<string> SaveCoverAsync(IFormFile Cover)
        {
            //Unique cover Name -- img stored on serve with this Name
            /*
               Avoiding Overwrites => multiple users can upload files with the same name without overwriting each other's files.
               Security => 
               {
                  directory traversal attacks or 
                   the execution of malicious files.
                  --> By renaming files to unique names, you minimize the risk associated with user-supplied names.
               }                 
               Organization => help with organizing and managing files on the server.
               Concurrency  => In a multi-user environment, generating unique file names helps avoid conflicts
                               that could arise when multiple users try to upload files with the same name simultaneously.

             */
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(Cover.FileName)}";

            // wwwroot/assets/images/games/example.jpg
            var path = Path.Combine(_imagePath, coverName);

            /*
             File.Create(path)  => creates a new file at the specified path and returns a [FileStream] that can be used to write data to that file.
             The using statement is used to ensure that the stream is properly disposed of when it's no longer needed. 
             This helps with resource management and ensures that the file stream is closed and resources are released correctly.
             -->[hard disk is not under the control of CLR so there is garbage collector]
             */
            using var stream = File.Create(path);

            /*
             viewModel.Cover => represents an uploaded file as part of a form submission
             CopyToAsync(stream) => is an asynchronous method that copies the contents of the uploaded file (viewModel.Cover) to the stream (the FileStream created earlier).

             */
            await Cover.CopyToAsync(stream);

            /*
             stream.Dispose();
             This line explicitly disposes of the stream (the FileStream) to release any resources held by it. 
             It's generally a good practice to call Dispose() on streams or use the using statement to ensure proper resource cleanup.
             Disposing the stream ensures that the file is properly closed and any buffered data is flushed to the file system.
             stream.Dispose();
             */


            return coverName;
        }
    }

}
