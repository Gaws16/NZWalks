using AutoMapper;
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
        }
    }
}
