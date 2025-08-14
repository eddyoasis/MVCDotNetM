using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCWebApp.Constants;
using MVCWebApp.Enums;
using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models.EmailGroups;
using MVCWebApp.Services;
using MVCWebApp.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVCWebApp.Controllers
{
    public class EmailGroupController(
        IEmailGroupService _emailGroupService,
        IMapModel _mapper
        ) : ControllerBase
    {
        static readonly List<SelectListItem> _typeSelections = ConverterHelper.ToSelectList<EmailGroupTypeEnum>();
        static readonly List<SelectListItem> _typeSearchSelections = ConverterHelper.ToSelectList<EmailGroupTypeSearchEnum>();

        public async Task<IActionResult> Index()
        {
            return View(new EmailGroupSearchReq
            {
                PageNumber = 1,
                PageSize = AppConstants.DefaultPageSize,
                TypeSelections = _typeSearchSelections
            });
        }

        [HttpPost]
        public async Task<IActionResult> SearchEmailGroups([FromBody] EmailGroupSearchReq req)
        {
            var paginatedResult = await _emailGroupService.GetAllAsync(req);

            return PartialView("_Search", paginatedResult);
        }

        public IActionResult Create()
        {
            var req = new EmailGroupAddReq
            {
                TypeSelections = _typeSelections
            };

            return PartialView("_CreatePartial", req);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmailGroupAddReq req)
        {
            req.TypeSelections = _typeSelections;

            //if(req.TypeID == (int)EmailGroupTypeEnum.AutoMargin)
            //{
            //    if (req.EmailCC.IsNullOrEmpty())
            //    {
            //        ModelState.AddModelError("EmailTo must have value", "EmailTo must have value");
            //        return PartialView("_EditPartial", req);
            //    }
            //}

            //if (req.TypeID == (int)EmailGroupTypeEnum.StockLoss)
            //{
            //    if (req.EmailTo.IsNullOrEmpty())
            //    {
            //        ModelState.AddModelError("EmailTo must have value", "EmailTo must have value");
            //        return PartialView("_EditPartial", req);
            //    }

            //}

            if (ModelState.IsValid)
            {
                await _emailGroupService.AddAsync(req);

                return Json(new { success = true });
            }
            return PartialView("_CreatePartial", req);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _emailGroupService.GetByEntityIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            var req = _mapper.MapDto<EmailGroupEditReq>(entity);
            req.TypeSelections = _typeSelections;

            return PartialView("_EditPartial", req);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmailGroupEditReq req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = await _emailGroupService.GetByEntityIdAsync(req.ID);
                    if (entity == null)
                    {
                        return NotFound();
                    }

                    await _emailGroupService.UpdateAsync(req, entity);

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
            var entity = await _emailGroupService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_DetailPartial", entity);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _emailGroupService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_DeletePartial", entity);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, EmailGroupViewModel viewModel)
        {
            var entity = await _emailGroupService.GetByEntityIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                await _emailGroupService.DeleteAsync(entity);

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
