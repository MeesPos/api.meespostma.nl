using AutoMapper;
using api.meespostma.nl.Data;
using api.meespostma.nl.Models.Projects;
using api.meespostma.nl.Models.User;

namespace api.meespostma.nl.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<ProjectCreateDto, Project>().ReverseMap();
            CreateMap<ProjectReadOnlyDto, Project>().ReverseMap();
            CreateMap<ProjectUpdateDto, Project>().ReverseMap();

            CreateMap<ApiUser, UserDto>().ReverseMap();
        }
    }
}
