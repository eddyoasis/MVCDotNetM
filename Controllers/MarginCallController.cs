using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCWebApp.BackgroundServices;
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
        IMapModel _mapper
        ) : ControllerBase
    {
        [HttpPost]
        public async Task TestSendEmailQueue()
        {
            /* test dequeue */
            taskQueue.QueueBackgroundWorkItem(async token =>
            {
                await Task.Delay(2000, token); // Simulate work
                Console.WriteLine($"Processed at: {DateTime.Now}");
            });
        }

        public async Task<IActionResult> Index()
        {
            var currencySearchEnum = ConverterHelper.ToSelectList<CurrencySearchEnum>();
            var marginCallSearchStatusEnum = ConverterHelper.ToSelectList<MarginCallSearchStatusEnum>();

            return View(new MarginCallSearchReq
            {
                PageNumber = 1,
                PageSize = 5,
                CcyCode = currencySearchEnum,
                Status = marginCallSearchStatusEnum
            });
        }

        [HttpPost]
        public async Task<IActionResult> SearchMarginCalls([FromBody] MarginCallSearchReq req)
        {
            var paginatedResult = await _marginCallService.GetAllAsync(req);

            return PartialView("_Search", paginatedResult);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _marginCallService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_DetailPartial", entity);
        }

        [Authorize]
        public async Task<IActionResult> Approve(int id)
        {
            var entity = await _marginCallService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_ApprovePartial", entity);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(MarginCallViewModel model)
        {
            if (model.ID > 0)
            {
                try
                {
                    var entity = await _marginCallService.GetByEntityIdAsync(model.ID);
                    if (entity == null)
                    {
                        return NotFound();
                    }

                    await _marginCallService.ApproveAsync(entity);

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error approving record: " + ex.Message);
                }
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
            if (model.ID > 0)
            {
                try
                {
                    var entity = await _marginCallService.GetByEntityIdAsync(model.ID);
                    if (entity == null)
                    {
                        return NotFound();
                    }

                    await _marginCallService.RejectAsync(entity);

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error rejecting record: " + ex.Message);
                }
            }
            return PartialView("_RejectPartial", model);
        }
    }
}
