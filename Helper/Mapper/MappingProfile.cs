using AutoMapper;
using MVCWebApp.Enums;
using MVCWebApp.Models;
using MVCWebApp.Models.AuditLogs;
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

            CreateMap<MarginFormula, MarginFormulaViewModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => GetEnumStringValue<FormulaTypeEnum>(src.Type)));
            CreateMap<MarginFormulaAddReq, MarginFormula>();
            CreateMap<MarginFormula, MarginFormulaEditReq>();

            CreateMap<MarginCall, MarginCallViewModel>();
            CreateMap<MarginCallDto, MarginCallViewModel>();

            CreateMap<AuditLog, AuditLogViewModel>()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => GetEnumStringValue<AuditLogTypeEnum>(src.TypeID)))
               .ForMember(dest => dest.Action, opt => opt.MapFrom(src => GetEnumStringValue<AuditLogActionEnum>(src.ActionID)));

            CreateMap<MarginFormulaSearchReq, BaseSearchReq>();
            CreateMap<EmailNotificationSearchReq, BaseSearchReq>();
            CreateMap<MarginCallSearchReq, BaseSearchReq>();
            CreateMap<AuditLogSearchReq, BaseSearchReq>();
        }
    }
}
