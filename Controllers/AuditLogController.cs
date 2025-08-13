using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCWebApp.Constants;
using MVCWebApp.Enums;
using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models.EmailNotifications;
using MVCWebApp.Models.MarginFormulas;
using MVCWebApp.Services;
using MVCWebApp.ViewModels;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;

namespace MVCWebApp.Controllers
{
    public class AuditLogController(
        IAuditLogService _auditLogService,
        IMapModel _mapper
        ) : ControllerBase
    {
        static readonly List<SelectListItem> _auditLogTypes = ConverterHelper.ToSelectList<AuditLogTypeEnum>();
        static readonly List<SelectListItem> _auditLogActions = ConverterHelper.ToSelectList<AuditLogActionEnum>();

        public async Task<IActionResult> Index()
        {
            return View(new AuditLogSearchReq
            {
                TypeSelections = _auditLogTypes,
                ActionSelections = _auditLogActions,
                PageNumber = 1,
                PageSize = AppConstants.DefaultPageSize
            });
        }

        [HttpPost]
        public async Task<IActionResult> SearchAuditLogs([FromBody] AuditLogSearchReq req)
        {
            var paginatedResult = await _auditLogService.GetAllAsync(req);

            return PartialView("_Search", paginatedResult);
        }

        [Authorize]
        public async Task<IActionResult> DetailsMTM(int id)
        {
            object response = null;

            var entity = await _auditLogService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            if(entity.TypeID == (int)AuditLogTypeEnum.AutoMarginMTM)
            {
                response = JsonConvert.DeserializeObject<MarginCallViewModel>(entity.NewValue);
                return PartialView("_DetailPartialMTM", response);
            }
            else if (entity.TypeID == (int)AuditLogTypeEnum.AutoMarginEOD)
            {
                response = JsonConvert.DeserializeObject<MarginCallViewModel>(entity.NewValue);
                return PartialView("_DetailPartialEOD", response);
            }
            else if (entity.TypeID == (int)AuditLogTypeEnum.Formula)
            {
                var sssasd = JsonConvert.DeserializeObject<MarginFormula>(entity.NewValue);
                response = _mapper.MapDto<MarginFormulaViewModel>(sssasd);
                return PartialView("_DetailPartialFormula", response);
            }
            else
            {
                response = JsonConvert.DeserializeObject<EmailNotificationViewModel>(entity.NewValue);
                return PartialView("_DetailPartialEmailNotification", response);
            }
        }
    }
}
