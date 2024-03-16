using AutoMapper;
using NZWalks.API.Controllers;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.MapperProfiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region,RegionsDTO>().ReverseMap();
            CreateMap<Region,CreateRegionRequestDTO>().ReverseMap();
            CreateMap<Region,UpdateRegionRequestDTO>().ReverseMap();
            CreateMap<Walk,AddWalkRequestDTO>().ReverseMap();
            CreateMap<Walk,WalkDTO>().ReverseMap();
            CreateMap<Difficulty,DifficultyDTO>().ReverseMap();
            CreateMap<UpdateWalkRequestDTO,Walk>().ReverseMap();
        }
    }
}
