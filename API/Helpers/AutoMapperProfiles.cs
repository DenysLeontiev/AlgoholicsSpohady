using API.DataTransferObjects;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDto>().ForMember(dest => dest.Memories, opts => opts.MapFrom(src => src.Memories))
                                      .ReverseMap();

            // CreateMap<Memory, MemoryDto>().ForMember(dest => dest.Users, opts => opts.MapFrom(src => src.Users))
            //                               .ReverseMap();

            CreateMap<Memory, MemoryDto>().ReverseMap();

            CreateMap<Photo, PhotoDto>().ReverseMap();

            CreateMap<Message, MessageDto>().ReverseMap();
        }
    }
}