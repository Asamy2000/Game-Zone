using AutoMapper;
using GameZone.Models;
using GameZone.ViewModels;

namespace GameZone.MappingProfiles
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<CreateGameViewModel, Game>().ReverseMap();
        }
    }
}
