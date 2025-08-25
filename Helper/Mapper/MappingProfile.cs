using AutoMapper;
using MVCWebApp.Enums;
using MVCWebApp.Models;
using MVCWebApp.Models.AuditLogs;
using MVCWebApp.Models.EmailGroups;
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
            CreateMap<EmailNotification, EmailNotificationEditReq>();
            CreateMap<EmailNotification, EmailNotificationViewModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => GetEnumStringValue<EmailNotificationTypeEnum>(src.TypeID)));

            CreateMap<MarginFormula, MarginFormulaViewModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => GetEnumStringValue<FormulaTypeEnum>(src.Type)));
            CreateMap<MarginFormulaAddReq, MarginFormula>();
            CreateMap<MarginFormula, MarginFormulaEditReq>();

            CreateMap<MarginCall, MarginCallViewModel>();
            CreateMap<MarginCallDto, MarginCallViewModel>()
                .ForMember(dest => dest.Percentages, opt => opt.MapFrom(src => src.Percentages.ToString("0.00")))
                .ForMember(dest => dest.Collateral, opt => opt.MapFrom(src => src.Collateral.ToString("0.00")))
                .ForMember(dest => dest.VM, opt => opt.MapFrom(src => src.VM.ToString("0.00")))
                .ForMember(dest => dest.MarginCallAmount, opt => opt.MapFrom(src => src.MarginCallAmount.ToString("0.00")));

            CreateMap<AuditLog, AuditLogViewModel>()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => GetEnumStringValue<AuditLogTypeEnum>(src.TypeID)))
               .ForMember(dest => dest.Action, opt => opt.MapFrom(src => GetEnumStringValue<AuditLogActionEnum>(src.ActionID)));

            CreateMap<EmailGroupAddReq, EmailGroup>();
            CreateMap<EmailGroup, EmailGroupEditReq>();
            CreateMap<EmailGroup, EmailGroupViewModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => GetEnumStringValue<EmailGroupTypeEnum>(src.TypeID)));

            CreateMap<MarginFormulaSearchReq, BaseSearchReq>();
            CreateMap<EmailNotificationSearchReq, BaseSearchReq>();
            CreateMap<MarginCallSearchReq, BaseSearchReq>();
            CreateMap<AuditLogSearchReq, BaseSearchReq>();
            CreateMap<EmailGroupSearchReq, BaseSearchReq>();
        }
    }
}
