using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCWebApp.Constants;
using MVCWebApp.Enums;
using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models.MarginFormulas;
using MVCWebApp.Services;
using MVCWebApp.ViewModels;
using System.Collections.Generic;

namespace MVCWebApp.Controllers
{
    public class MarginFormulaController(
        IMarginFormulaService _marginFormulaService,
        IMapModel _mapper
        ) : ControllerBase
    {
        static readonly List<SelectListItem>  _typeSelections = ConverterHelper.ToSelectList<FormulaTypeEnum>();
        static readonly List<SelectListItem>  _typeSearchSelections = ConverterHelper.ToSelectList<FormulaTypeSearchEnum>();

        public async Task<IActionResult> Index()
        {
            return View(new MarginFormulaSearchReq
            {
                PageNumber = 1,
                PageSize = AppConstants.DefaultPageSize,
                TypeSelections = _typeSearchSelections
            });
        }

        [HttpPost]
        public async Task<IActionResult> SearchMarginFormulas([FromBody] MarginFormulaSearchReq req)
        {
            var paginatedResult = await _marginFormulaService.GetAllAsync(req);

            return PartialView("_Search", paginatedResult);
        }

        public IActionResult Create()
        {
            var req = new MarginFormulaAddReq
            {
                TypeSelections = _typeSelections
            };

            return PartialView("_CreatePartial", req);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MarginFormulaAddReq req)
        {
            if (ModelState.IsValid)
            {
                await _marginFormulaService.AddAsync(req);

                return Json(new { success = true });
            }
            return PartialView("_CreatePartial", req);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _marginFormulaService.GetByEntityIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            var req = _mapper.MapDto<MarginFormulaEditReq>(entity);
            req.TypeSelections = _typeSelections;


            return PartialView("_EditPartial", req);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MarginFormulaEditReq req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = await _marginFormulaService.GetByEntityIdAsync(req.ID);
                    if (entity == null)
                    {
                        return NotFound();
                    }

                    await _marginFormulaService.UpdateAsync(req, entity);

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
            var entity = await _marginFormulaService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_DetailPartial", entity);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _marginFormulaService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_DeletePartial", entity);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, MarginFormulaViewModel viewModel)
        {
            var entity = await _marginFormulaService.GetByEntityIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                await _marginFormulaService.DeleteAsync(entity);

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
