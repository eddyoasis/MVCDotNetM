using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCWebApp.BackgroundServices;
using MVCWebApp.Constants;
using MVCWebApp.Enums;
using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models.MarginCalls;
using MVCWebApp.Services;
using MVCWebApp.ViewModels;

namespace MVCWebApp.Controllers
{
    public class MarginCallController(
        IBackgroundTaskQueue taskQueue,
        IMarginCallService _marginCallService,
        IEmailNotificationService _emailNotificationService,
        IEmailService _emailService,
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

            return View(new MarginCallSearchReq
            {
                PageNumber = 1,
                PageSize = AppConstants.DefaultPageSize,
                Collateral_Ccy = collateralCcy,
                IM_Ccy = imCcy,
                VM_Ccy = vmCcy,
                EODTriggerFlag = yesNoEnum,
                MarginCallFlag = yesNoEnum,
                MTMTriggerFlag = yesNoEnum
            });
        }

        [HttpPost]
        public async Task<IActionResult> SearchMarginCalls([FromBody] MarginCallSearchReq req)
        {
            var paginatedResult = await _marginCallService.GetAllAsync(req);

            return PartialView("_Search", paginatedResult);
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            var entity = await _marginCallService.GetByPortfolioID(id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_DetailPartial", entity);
        }

        [Authorize]
        public async Task<IActionResult> Approve(string id)
        {
            var entity = await _marginCallService.GetByPortfolioID(id);
            if (entity == null)
            {
                return NotFound();
            }

            var emailNotifications = await _emailNotificationService.GetAllAsync();
            if (emailNotifications.Any())
            {
                entity.EmailTemplateList =
                        emailNotifications
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
                    var entity = await _marginCallService.GetEntityByPortfolioID(model.PortfolioID);
                    if (entity == null)
                    {
                        return NotFound();
                    }

                    var isSuccess = await _marginCallService.ApproveWithSP(model);

                    if (isSuccess)
                    {
                        return Json(new { success = true });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error approving record: " + ex.Message);
                }
            }

            var emailNotifications = await _emailNotificationService.GetAllAsync();
            if (emailNotifications.Any())
            {
                model.EmailTemplateList =
                        emailNotifications
                        .Select(x => new SelectListItem
                        {
                            Text = x.MarginType,
                            Value = x.EmailTemplate
                        })
                        .ToList();
            }

            return PartialView("_ApprovePartial", model);
        }

        [Authorize]
        public async Task<IActionResult> Reject(int id)
        {
            var entity = await _marginCallService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_RejectPartial", entity);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(MarginCallViewModel model)
        {
            //if (model.ID > 0)
            //{
            //    try
            //    {
            //        var entity = await _marginCallService.GetByEntityIdAsync(model.ID);
            //        if (entity == null)
            //        {
            //            return NotFound();
            //        }

            //        await _marginCallService.RejectAsync(entity);

            //        return Json(new { success = true });
            //    }
            //    catch (Exception ex)
            //    {
            //        ModelState.AddModelError("", "Error rejecting record: " + ex.Message);
            //    }
            //}
            return PartialView("_RejectPartial", model);
        }
    }
}
