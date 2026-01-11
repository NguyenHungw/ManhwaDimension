using AutoMapper;
using ManhwaDimension.Models;
using ManhwaDimension.Models.DTO.Comic;
using ManhwaDimension.Util.Extentions;

namespace ManhwaDimension
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ComicDTO, Comic>()
            // Slug từ Title
            .ForMember(dest => dest.Slug,
                opt => opt.MapFrom(src => StringExtension.ConvertToSlug(src.Title)))

            // CreatedAt, UpdatedAt
            .ForMember(dest => dest.CreatedAt,
                opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => DateTime.Now))

            // CoverImageUrl set sau
            .ForMember(dest => dest.CoverImageUrl, opt => opt.Ignore())

            // Id tự tạo
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Nếu DTO không có field này, set mặc định
            //.ForMember(dest => dest.IsDeleted,
            //    opt => opt.MapFrom(src => src.IsDeleted ?? false))
            //.ForMember(dest => dest.Active,
            //    opt => opt.MapFrom(src => src.Active ?? true))

            //// Map authorId từ DTO sang AuthorId trong Entity
            //.ForMember(dest => dest.AuthorId,
            //    opt => opt.MapFrom(src => src.authorId));
        }
    }
}
