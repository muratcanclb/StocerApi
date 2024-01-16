using AutoMapper;
using Meb.Core.Infrastructure.Mapper;

namespace Intra.Api.Infrastructure.Mapper
{
    public class MapperClass : Profile, IMapperProfile
    {
        public MapperClass()
        {
            //CreateMap<USERMODEL, UMS_USER>()
            //      .ForMember(dest => dest.CUSTOMER_ID, mo => mo.Ignore())
            //      .ForMember(dest => dest.TITLE, mo => mo.Ignore())
            //      .ForMember(dest => dest.EMAIL, mo => mo.Ignore())
            //      .ForMember(dest => dest.COUNTRY_CODE, mo => mo.Ignore())
            //      .ForMember(dest => dest.PHONE_NUMBER, mo => mo.Ignore())
            //      .ForMember(dest => dest.MOBILE_NUMBER, mo => mo.Ignore())
            //      .ForMember(dest => dest.FAX, mo => mo.Ignore())
            //      .ForMember(dest => dest.PASSWORD_RESET_CODE, mo => mo.Ignore())
            //      .ForMember(dest => dest.RESET_CODE_EXPIRATION_DATE, mo => mo.Ignore())
            //      .ForMember(dest => dest.LAST_LOGIN_DATE, mo => mo.Ignore())
            //      .ForMember(dest => dest.IS_ACTIVE, mo => mo.Ignore())
            //      .ForMember(dest => dest.CREATED_DATE, mo => mo.Ignore())
            //      .ForMember(dest => dest.CREATED_BY, mo => mo.Ignore())
            //      .ForMember(dest => dest.UPDATED_DATE, mo => mo.Ignore())
            //      .ForMember(dest => dest.UPDATED_BY, mo => mo.Ignore())
            //      .ForMember(dest => dest.IS_DELETED, mo => mo.Ignore());

            //CreateMap<LoginFailedAttempDto, UMS_LOGIN_FAILED_ATTEMPT>()
            //      .ForMember(dest => dest.XX_ID, mo => mo.Ignore());
            //CreateMap<UserDto, UMS_USER>()
            //     .ForMember(dest => dest.XX_ID, mo => mo.Ignore());
            //CreateMap<RoleDto, UMS_USER>()
            //    .ForMember(dest => dest.XX_ID, mo => mo.Ignore());
            //CreateMap<ProjectDto, UMS_PROJECT>()
            //    .ForMember(dest => dest.XX_ID, mo => mo.Ignore());
            //CreateMap<PermissionDto, UMS_PERMISSION>()
            //   .ForMember(dest => dest.XX_ID, mo => mo.Ignore());
            //CreateMap<GroupDto, UMS_GROUP>()
            //   .ForMember(dest => dest.XX_ID, mo => mo.Ignore());
            //CreateMap<CustomerDto, UMS_CUSTOMER>()
            //  .ForMember(dest => dest.XX_ID, mo => mo.Ignore());

            //CreateMap<UMS_LOGIN_FAILED_ATTEMPT, LoginFailedAttempDto>()
            //      .ForMember(dest => dest.RegisterId, opt => opt.MapFrom(src => src.REGISTER_ID))
            //      .ForMember(dest => dest.AccessFailedCount, opt => opt.MapFrom(src => src.ACCESS_FAILED_COUNT))
            //      .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IP_ADDRESS))
            //      .ForMember(dest => dest.LastLoginFailedTime, opt => opt.MapFrom(src => src.LAST_FAILED_DATE))
            //      .ForMember(dest => dest.IsCaptcha, opt => opt.MapFrom(src => src.IS_CAPTCHA))
            //      .ForMember(dest => dest.IsLock, opt => opt.MapFrom(src => src.IS_LOCKED))
            //      .ForMember(dest => dest.LockTime, opt => opt.MapFrom(src => src.LOCKED_TIME));

        }

        public int Order => 0;
    }
}