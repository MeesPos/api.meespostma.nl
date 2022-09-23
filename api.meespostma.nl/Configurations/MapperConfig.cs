using AutoMapper;
using api.meespostma.nl.Data;
using api.meespostma.nl.Models.Projects;

namespace api.meespostma.nl.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<ProjectCreateDto, Project>().ReverseMap();
            CreateMap<ProjectReadOnlyDto, Project>().ReverseMap();
            CreateMap<ProjectUpdateDto, Project>().ReverseMap();
        }
    }
}
