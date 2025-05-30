using AutoMapper;
using MVCWebApp.Models;
using MVCWebApp.Models.EmailNotifications;
using MVCWebApp.Models.MarginCalls;
using MVCWebApp.Models.MarginFormulas;
using MVCWebApp.ViewModels;

namespace MVCWebApp.Helper.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmailNotificationAddReq, EmailNotification>();
            CreateMap<EmailNotification, EmailNotificationViewModel>();
            CreateMap<EmailNotification, EmailNotificationEditReq>();

            CreateMap<MarginFormula, MarginFormulaViewModel>();
            CreateMap<MarginFormulaAddReq, MarginFormula>();
            CreateMap<MarginFormula, MarginFormulaEditReq>();

            CreateMap<MarginCall, MarginCallViewModel>();

            CreateMap<MarginFormulaSearchReq, BaseSearchReq>();
            CreateMap<EmailNotificationSearchReq, BaseSearchReq>();
            CreateMap<MarginCallSearchReq, BaseSearchReq>();
        }
    }
}
