using GameZone.CustomValidation;
using GameZone.Models;
using GameZone.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GameZone.ViewModels
{
    /*
       category => dropdown list and User can select One [each game have one category]
       devices =>  dropdown list and User can select more than one device [one game have a collection of devices]
     */
    public class CreateGameViewModel : GameViewModel
    {
       
        [AllowedExtensions(FileSettings.AllowedExtentions)]
        [MaxFileSize(FileSettings.MaxFileSizeInByte)]
        public IFormFile Cover { get; set; } = default!;

    }
}
