using AutoMapper;
using api.meespostma.nl.Data;
using api.meespostma.nl.Models.Projects;

namespace api.meespostma.nl.Configurations
{
    public class MapperConfig : Profile
    {
        CreateMap<ProjectCreateDto, Project>().ReverseMap();
    }
}
