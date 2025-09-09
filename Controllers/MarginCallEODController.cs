using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCWebApp.BackgroundServices;
using MVCWebApp.Constants;
using MVCWebApp.Enums;
using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models.AuditLogs;
using MVCWebApp.Models.MarginCalls;
using MVCWebApp.Services;
using MVCWebApp.ViewModels;
using Newtonsoft.Json;

namespace MVCWebApp.Controllers
{
    public class MarginCallEODController(
        IBackgroundTaskQueue taskQueue,
        IMarginCallService _marginCallService,
        IEmailNotificationService _emailNotificationService,
        IEmailService _emailService,
        IEmailGroupService _emailGroupService,
        IAuditLogService _auditLogService,
        IMapModel _mapper
        ) : ControllerBase
    {
        public async Task<IActionResult> Index()
        {
            var currencySearchEnum = ConverterHelper.ToSelectList<CurrencySearchEnum>();
            var marginCallSearchStatusEnum = ConverterHelper.ToSelectList<MarginCallSearchStatusEnum>();
            var yesNoEnum = ConverterHelper.ToSelectList<YesNoEnum>();

            var collateralCcy = ConverterHelper.ToSelectList(await _marginCallService.GetAllCollateralCcy());
            var imCcy = ConverterHelper.ToSelectList(await _marginCallService.GetAllIMCcy());
            var vmCcy = ConverterHelper.ToSelectList(await _marginCallService.GetAllVMCcy());

            var marginCallOrderByColumns = ConverterHelper.ToSelectList<MarginCallOrderByColumn>();
            var marginCallEODDays = ConverterHelper.ToSelectList<MarginCallEODDay>();

            return View(new MarginCallSearchReq
            {
                PageNumber = 1,
                PageSize = AppConstants.DefaultPageSize,
                Collateral_Ccy = collateralCcy,
                IM_Ccy = imCcy,
                VM_Ccy = vmCcy,
                EODTriggerFlag = yesNoEnum,
                MarginCallFlag = yesNoEnum,
                MTMTriggerFlag = yesNoEnum,
                SelectedMarginCallFlag = 2,
                MarginCallOrderByColumns = marginCallOrderByColumns,
                Day = marginCallEODDays
            });
        }

        [HttpPost]
        public async Task<IActionResult> SearchMarginCalls([FromBody] MarginCallSearchReq req)
        {
            var paginatedResult = _marginCallService.GetMarginCallEODAll(req);

            ViewData["isMaginModeAll"] = 1;

            return PartialView("_Search", paginatedResult);
        }

        [Authorize]
        public async Task<IActionResult> StoplossIMDetail(string id)
        {
            var result = _marginCallService.GetEODIMProduct(id);
            if (result == null)
            {
                return NotFound();
            }

            return PartialView("_StoplossIMDetailPartial", result);
        }

        [Authorize]
        public async Task<IActionResult> ResetFlag(string id)
        {
            if (id.IsNotNullOrEmpty())
            {
                try
                {
                    var entity = _marginCallService.GetMarginCallEOD(id);
                    if (entity == null)
                    {
                        return NotFound();
                    }

                    var isSuccess = await _marginCallService.ResetFlagEOD(id);
                    if (isSuccess)
                    {
                        var auditReq = new AuditLog
                        {
                            TypeID = (int)AuditLogTypeEnum.AutoMarginEOD,
                            ActionID = (int)AuditLogActionEnum.ResetFlag,
                            Name = entity.PortfolioID,
                            CreatedBy = Username,
                            CreatedAt = DateTime.Now,
                            NewValue = JsonConvert.SerializeObject(entity)
                        };

                        await _auditLogService.AddAsync(auditReq);

                        return Json(new { success = true });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error Reset flag: " + ex.Message);
                }
            }

            return Json(new { success = false });
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            var entity = _marginCallService.GetMarginCallEOD(id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_DetailPartial", entity);
        }

        [Authorize]
        public async Task<IActionResult> ApproveMOC(string id)
        {
            var entity = _marginCallService.GetMarginCallEOD(id);
            if (entity == null)
            {
                return NotFound();
            }

            var emailGroups = await _emailGroupService.GetAllAsync();
            if (emailGroups.Any())
            {
                entity.EmailGroupSelections =
                    emailGroups
                    .Where(x => x.TypeID == (int)EmailGroupTypeEnum.MOC)
                    .ToList();
            }

            var emailNotifications = await _emailNotificationService.GetAllAsync();
            if (emailNotifications.Any())
            {
                entity.EmailTemplateList =
                        emailNotifications
                        .Where(x => x.TypeID == (int)EmailNotificationTypeEnum.MOC)
                        .Select(x => new SelectListItem
                        {
                            Text = x.MarginType,
                            Value = x.EmailTemplate
                        })
                        .ToList();
            }

            return PartialView("_ApproveMOCPartial", entity);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveMOC(MarginCallViewModel model)
        {
            if (model.PortfolioID.IsNotNullOrEmpty())
            {
                try
                {
                    var entity = _marginCallService.GetMarginCallEOD(model.PortfolioID);
                    if (entity == null)
                    {
                        return NotFound();
                    }

                    _mapper.Map(entity, model);

                    var isSuccess = await _marginCallService.ApproveMarginCallEODMOC(model);
                    if (isSuccess)
                    {
                        var auditReq = new AuditLog
                        {
                            TypeID = (int)AuditLogTypeEnum.AutoMarginEOD,
                            ActionID = (int)AuditLogActionEnum.ApproveMOC,
                            Name = model.PortfolioID,
                            CreatedBy = Username,
                            CreatedAt = DateTime.Now,
                            NewValue = JsonConvert.SerializeObject(model)
                        };

                        await _auditLogService.AddAsync(auditReq);

                        return Json(new { success = true });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error approving record: " + ex.Message);
                }
            }

            var emailGroups = await _emailGroupService.GetAllAsync();
            if (emailGroups.Any())
            {
                model.EmailGroupSelections =
                    emailGroups
                    .Where(x => x.TypeID == (int)EmailGroupTypeEnum.MOC)
                    .ToList();
            }

            var emailNotifications = await _emailNotificationService.GetAllAsync();
            if (emailNotifications.Any())
            {
                model.EmailTemplateList =
                        emailNotifications
                        .Where(x => x.TypeID == (int)EmailNotificationTypeEnum.MOC)
                        .Select(x => new SelectListItem
                        {
                            Text = x.MarginType,
                            Value = x.EmailTemplate
                        })
                        .ToList();
            }

            return PartialView("_ApproveMOCPartial", model);
        }

        [Authorize]
        public async Task<IActionResult> ApproveStoploss(string id)
        {
            var entity = _marginCallService.GetMarginCallEOD(id);
            if (entity == null)
            {
                return NotFound();
            }

            var emailGroups = await _emailGroupService.GetAllAsync();
            if (emailGroups.Any())
            {
                entity.EmailGroupSelections =
                    emailGroups
                    .Where(x => x.TypeID == (int)EmailGroupTypeEnum.StopLoss)
                    .ToList();
            }

            var emailNotifications = await _emailNotificationService.GetAllAsync();
            if (emailNotifications.Any())
            {
                entity.EmailTemplateList =
                        emailNotifications
                        .Where(x => x.TypeID == (entity.Day == "2" ? (int)EmailNotificationTypeEnum.Day2Stoploss : (int)EmailNotificationTypeEnum.Day3Stoploss)) 
                        .Select(x => new SelectListItem
                        {
                            Text = x.MarginType,
                            Value = x.EmailTemplate
                        })
                        .ToList();
            }

            var stoplossOrderDetail = _marginCallService.GetEODStoplossOrderDetail(id);
            entity.StoplossOrderDetail = stoplossOrderDetail?.Action;

            return PartialView("_ApproveStoplossPartial", entity);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveStoploss(MarginCallViewModel model)
        {
            if (model.PortfolioID.IsNotNullOrEmpty())
            {
                try
                {
                    var entity = _marginCallService.GetMarginCallEOD(model.PortfolioID);
                    if (entity == null)
                    {
                        return NotFound();
                    }

                    _mapper.Map(entity, model);

                    var isSuccess = await _marginCallService.ApproveMarginCallEODStoploss(model);
                    if (isSuccess)
                    {
                        var auditReq = new AuditLog
                        {
                            TypeID = (int)AuditLogTypeEnum.AutoMarginEOD,
                            ActionID = (int)AuditLogActionEnum.ApproveStoploss,
                            Name = model.PortfolioID,
                            CreatedBy = Username,
                            CreatedAt = DateTime.Now,
                            NewValue = JsonConvert.SerializeObject(model)
                        };

                        await _auditLogService.AddAsync(auditReq);

                        return Json(new { success = true });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error approving record: " + ex.Message);
                }
            }

            var emailGroups = await _emailGroupService.GetAllAsync();
            if (emailGroups.Any())
            {
                model.EmailGroupSelections =
                    emailGroups
                    .Where(x => x.TypeID == (int)EmailGroupTypeEnum.StopLoss)
                    .ToList();
            }

            var emailNotifications = await _emailNotificationService.GetAllAsync();
            if (emailNotifications.Any())
            {
                model.EmailTemplateList =
                        emailNotifications
                        .Where(x => x.TypeID == (int)EmailNotificationTypeEnum.Day2Stoploss)
                        .Select(x => new SelectListItem
                        {
                            Text = x.MarginType,
                            Value = x.EmailTemplate
                        })
                        .ToList();
            }

            return PartialView("_ApproveStoplossPartial", model);
        }

        [Authorize]
        public async Task<IActionResult> Approve(string id)
        {
            var entity = _marginCallService.GetMarginCallEOD(id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.EmailTo = _marginCallService.GetClientEmail(id).Email;

            var emailGroups = await _emailGroupService.GetAllAsync();
            if (emailGroups.Any())
            {
                entity.EmailGroupSelections =
                    emailGroups
                    .Where(x => x.TypeID == (int)EmailGroupTypeEnum.AutoMargin)
                    .ToList();
            }

            var emailNotifications = await _emailNotificationService.GetAllAsync();
            if (emailNotifications.Any())
            {
                entity.EmailTemplateList =
                        emailNotifications
                        .Where(x=>x.TypeID == Convert.ToInt32(entity.Day))
                        .Select(x => new SelectListItem
                        {
                            Text = x.MarginType,
                            Value = x.EmailTemplate
                        })
                        .ToList();
            }

            return PartialView("_ApprovePartial", entity);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(MarginCallViewModel model)
        {
            if (model.PortfolioID.IsNotNullOrEmpty())
            {
                try
                {
                    var entity = _marginCallService.GetMarginCallEOD(model.PortfolioID);
                    if (entity == null)
                    {
                        return NotFound();
                    }

                    _mapper.Map(entity, model);

                    var isSuccess = await _marginCallService.ApproveMarginCallEOD(model);
                    if (isSuccess)
                    {
                        var auditReq = new AuditLog
                        {
                            TypeID = (int)AuditLogTypeEnum.AutoMarginEOD,
                            ActionID = (int)AuditLogActionEnum.Approve,
                            Name = model.PortfolioID,
                            CreatedBy = Username,
                            CreatedAt = DateTime.Now,
                            NewValue = JsonConvert.SerializeObject(model)
                        };

                        await _auditLogService.AddAsync(auditReq);

                        return Json(new { success = true });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error approving record: " + ex.Message);
                }
            }

            var emailGroups = await _emailGroupService.GetAllAsync();
            if (emailGroups.Any())
            {
                model.EmailGroupSelections =
                    emailGroups
                    .Where(x => x.TypeID == (int)EmailGroupTypeEnum.AutoMargin)
                    .ToList();
            }

            var emailNotifications = await _emailNotificationService.GetAllAsync();
            if (emailNotifications.Any())
            {
                model.EmailTemplateList =
                        emailNotifications
                        .Where(x => x.TypeID == Convert.ToInt32(model.Day))
                        .Select(x => new SelectListItem
                        {
                            Text = x.MarginType,
                            Value = x.EmailTemplate
                        })
                        .ToList();
            }

            return PartialView("_ApprovePartial", model);
        }
    }
}
