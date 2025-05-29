using AutoMapper.Execution;
using AutoMapper;
using System.ComponentModel;
using System.Security.Principal;
using MVCWebApp.Models.EmailNotifications;
using MVCWebApp.ViewModels;

namespace MVCWebApp.Helper.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmailNotificationAddReq, EmailNotification>();
            CreateMap<EmailNotification, EmailNotificationViewModel>();
            //CreateMap<EmailNotificationViewModel, EmailNotification>();
            CreateMap<EmailNotification, EmailNotificationEditReq>(); 
        }
    }
}
