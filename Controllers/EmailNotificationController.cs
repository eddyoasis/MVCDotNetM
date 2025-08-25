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

namespace MVCWebApp.Controllers
{
    public class EmailNotificationController(
        IEmailNotificationService _emailNotificationService,
        IMapModel mapper
        ) : ControllerBase
    {
        static readonly List<SelectListItem> _typeSelections = ConverterHelper.ToSelectList<EmailNotificationTypeEnum>();
        static readonly List<SelectListItem> _typeSearchSelections = ConverterHelper.ToSelectList<EmailNotificationTypeSearchEnum>();

        public async Task<IActionResult> Index()
        {
            return View(new EmailNotificationSearchReq 
            { 
                PageNumber = 1, 
                PageSize = AppConstants.DefaultPageSize,
                TypeSelections = _typeSearchSelections
            });
        }

        [HttpPost]
        public async Task<IActionResult> SearchEmailNotifications([FromBody] EmailNotificationSearchReq req)
        {
            var paginatedResult = await _emailNotificationService.GetAllAsync(req);

            return PartialView("_Search", paginatedResult);
        }

        public IActionResult Create()
        {
            var req = new EmailNotificationAddReq
            {
                TypeSelections = _typeSelections
            };

            return PartialView("_CreatePartial", req);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmailNotificationAddReq req)
        {
            req.TypeSelections = _typeSelections;

            if (ModelState.IsValid)
            {
                await _emailNotificationService.AddAsync(req);

                return Json(new { success = true });
            }
            return PartialView("_CreatePartial", req);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _emailNotificationService.GetByEntityIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            var req = mapper.MapDto<EmailNotificationEditReq>(entity);
            req.TypeSelections = _typeSelections;

            return PartialView("_EditPartial", req);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmailNotificationEditReq req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = await _emailNotificationService.GetByEntityIdAsync(req.ID);
                    if (entity == null)
                    {
                        return NotFound();
                    }

                    await _emailNotificationService.Update(req, entity);

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating record: " + ex.Message);
                }
            }
            return PartialView("_EditPartial", req);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _emailNotificationService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_DetailPartial", entity);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _emailNotificationService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_DeletePartial", entity);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, EmailNotificationViewModel viewModel)
        {
            var entity = await _emailNotificationService.GetByEntityIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                await _emailNotificationService.Delete(entity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting record: " + ex.Message);
            }

            return PartialView("_DeletePartial", viewModel);
        }
    }
}
