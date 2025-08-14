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
                MarginCallOrderByColumns = marginCallOrderByColumns
            });
        }

        [HttpPost]
        public async Task<IActionResult> SearchMarginCalls([FromBody] MarginCallSearchReq req)
        {
            var paginatedResult = _marginCallService.GetMarginCallEODAll(req);

            ViewData["isMaginModeAll"] = req.Selected_MarginMode == 1 ? 1 : 0;

            return PartialView("_Search", paginatedResult);
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

                    var isSuccess = await _marginCallService.ApproveMarginCallEOD(model);
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
        public async Task<IActionResult> ApproveStockloss(string id)
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
                    .Where(x => x.TypeID == (int)EmailGroupTypeEnum.StockLoss)
                    .ToList();
            }

            var emailNotifications = await _emailNotificationService.GetAllAsync();
            if (emailNotifications.Any())
            {
                entity.EmailTemplateList =
                        emailNotifications
                        .Where(x => x.TypeID == (int)EmailNotificationTypeEnum.StockLoss)
                        .Select(x => new SelectListItem
                        {
                            Text = x.MarginType,
                            Value = x.EmailTemplate
                        })
                        .ToList();
            }

            return PartialView("_ApproveStocklossPartial", entity);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveStockloss(MarginCallViewModel model)
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
                            ActionID = (int)AuditLogActionEnum.ApproveStockloss,
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
                    .Where(x => x.TypeID == (int)EmailGroupTypeEnum.StockLoss)
                    .ToList();
            }

            var emailNotifications = await _emailNotificationService.GetAllAsync();
            if (emailNotifications.Any())
            {
                model.EmailTemplateList =
                        emailNotifications
                        .Where(x => x.TypeID == (int)EmailNotificationTypeEnum.StockLoss)
                        .Select(x => new SelectListItem
                        {
                            Text = x.MarginType,
                            Value = x.EmailTemplate
                        })
                        .ToList();
            }

            return PartialView("_ApproveStocklossPartial", model);
        }

        [Authorize]
        public async Task<IActionResult> Approve(string id)
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
