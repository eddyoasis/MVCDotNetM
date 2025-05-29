using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models.EmailNotifications;
using MVCWebApp.Services;
using MVCWebApp.ViewModels;

namespace MVCWebApp.Controllers
{
    public class EmailNotificationController(
        IEmailNotificationService _emailNotificationService,
        IMapModel mapper
        ) : ControllerBase
    {
        public async Task<IActionResult> Index()
        {
            return View(new EmailNotificationSearchReq { PageNumber = 1, PageSize = 5 });
        }

        [HttpPost]
        public async Task<IActionResult> SearchEmailNotifications([FromBody] EmailNotificationSearchReq req)
        {
            var paginatedResult = await _emailNotificationService.GetAllAsync(req);

            return PartialView("_Search", paginatedResult);
        }

        public IActionResult Create()
        {
            return PartialView("_CreatePartial", new EmailNotificationAddReq());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmailNotificationAddReq req)
        {
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

                    _emailNotificationService.Update(req, entity);

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
                _emailNotificationService.Delete(entity);

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
