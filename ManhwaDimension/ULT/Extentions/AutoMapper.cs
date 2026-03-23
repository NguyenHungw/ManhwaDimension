using AutoMapper;
using ManhwaDimension.Models;
using ManhwaDimension.Models.DTO.Comic;

//using ManhwaDimension.Models.DTO.Account;
using ManhwaDimension.Models.Response;
using Profile = AutoMapper.Profile;

namespace ManhwaDimension.Util.Extentions
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            //CreateMap<InsertAccountDTO, Account>();
            //CreateMap<Account, AccountProfileResponseDTO>();
            //CreateMap<UpdateAccountDTO, Account>();
            //CreateMap<Account, AccountProfileDTO>()
            //.ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.Province.Name))
            //.ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District.Name))
            //.ForMember(dest => dest.WardName, opt => opt.MapFrom(src => src.Ward.Name));

            // Map từ ComicDTO sang Comic
            CreateMap<ComicDTO, Comic>()
                // Slug tự động tạo từ Title
                .ForMember(dest => dest.Slug,
                    opt => opt.MapFrom(src => StringExtension.ConvertToSlug(src.Title)))
                // CreatedAt, UpdatedAt tự động set
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt,
                    opt => opt.MapFrom(src => DateTime.Now))
                // CoverImageUrl sẽ set sau khi upload
                .ForMember(dest => dest.CoverImageUrl, opt => opt.Ignore())
                // Id do database tự tạo
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Map ngược lại từ Comic sang ComicDTO (nếu cần)
            CreateMap<Comic, ComicDTO>();
        }
    }
}
