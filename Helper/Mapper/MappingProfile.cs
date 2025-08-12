using AutoMapper;
using MVCWebApp.Enums;
using MVCWebApp.Models;
using MVCWebApp.Models.EmailNotifications;
using MVCWebApp.Models.MarginCalls;
using MVCWebApp.Models.MarginFormulas;
using MVCWebApp.ViewModels;
using static MVCWebApp.Helper.EnumHelper;

namespace MVCWebApp.Helper.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmailNotificationAddReq, EmailNotification>();
            CreateMap<EmailNotification, EmailNotificationViewModel>();
            CreateMap<EmailNotification, EmailNotificationEditReq>();

            //CreateMap<MarginFormula, MarginFormulaViewModel>();
            CreateMap<MarginFormula, MarginFormulaViewModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => GetEnumStringValue<FormulaTypeEnum>(src.Type)));
            CreateMap<MarginFormulaAddReq, MarginFormula>();
            CreateMap<MarginFormula, MarginFormulaEditReq>();

            CreateMap<MarginCall, MarginCallViewModel>();
            CreateMap<MarginCallDto, MarginCallViewModel>();

            //CreateMap<MarginCall, MarginCallViewModel>()
            //    .ForMember(dest => dest.CcyCode, opt => opt.MapFrom(src => GetEnumStringValue<CurrencySearchEnum>(src.CcyCode)))
            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetEnumStringValue<MarginCallSearchStatusEnum>(src.Status)));

            CreateMap<MarginFormulaSearchReq, BaseSearchReq>();
            CreateMap<EmailNotificationSearchReq, BaseSearchReq>();
            CreateMap<MarginCallSearchReq, BaseSearchReq>();
        }
    }
}
