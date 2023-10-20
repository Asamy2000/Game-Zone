using GameZone.CustomValidation;
using GameZone.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GameZone.ViewModels
{
    public class EditGameViewModel : GameViewModel
    {
        public int Id { get; set; }

        public string? CurrentCover { get; set; }

        [AllowedExtensions(FileSettings.AllowedExtentions),
            MaxFileSize(FileSettings.MaxFileSizeInByte)]
        public IFormFile? Cover { get; set; } = default!;
    }
}
