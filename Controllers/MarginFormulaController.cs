using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Helper;
using MVCWebApp.Models.Req;
using MVCWebApp.ViewModels;

namespace MVCWebApp.Controllers
{
    public class MarginFormulaController: ControllerBase
    {
        List<MarginFormulaViewModel> _marginFormula =
            [
                new MarginFormulaViewModel
                {
                    ID = 1,
                    MarginFormula = "MarginFormula1",
                    MarginType = "MarginType1",
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin123",
                },
            new MarginFormulaViewModel
                {
                    ID = 2,
                    MarginFormula = "MarginFormula2",
                    MarginType = "MarginType2",
                    CreatedAt = DateTime.Now.AddHours(-5),
                    CreatedBy = "Admin123",
                     ModifiedAt = DateTime.Now,
                      ModifiedBy = "Admin123"
                },
            new MarginFormulaViewModel
                {
                    ID = 3,
                    MarginFormula = "MarginFormula3",
                    MarginType = "MarginType3",
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin123",
                }
            ];

        public async Task<IActionResult> Index()
        {
            return View(new MarginFormulaSearchReq { PageNumber = 1, PageSize = 5 });
        }

        [HttpPost]
        public async Task<IActionResult> SearchMarginFormulas([FromBody] MarginFormulaSearchReq req)
        {
            int pageNumber = req.PageNumber ?? 1;
            int pageSize = req.PageSize ?? 10;

            var paginatedResult = PaginatedList<MarginFormulaViewModel>.CreateAsync(
                _marginFormula,
                pageNumber,
                pageSize);

            return PartialView("_Search", paginatedResult);
        }

        public IActionResult Create()
        {
            return PartialView("_CreatePartial", new MarginFormulaAddReq());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MarginFormulaAddReq entity)
        {
            if (ModelState.IsValid)
            {
                return Json(new { success = true });
            }
            return PartialView("_CreatePartial", entity);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var entity = _marginFormula.FirstOrDefault(x => x.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            var res = new MarginFormulaEditReq
            {
                ID = entity.ID,
                MarginType = entity.MarginType,
                MarginFormula = entity.MarginFormula
            };

            return PartialView("_EditPartial", res);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MarginFormulaEditReq entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating record: " + ex.Message);
                }
            }
            return PartialView("_EditPartial", entity);
        }

        [Authorize]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _marginFormula.FirstOrDefault(x => x.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_DetailPartial", entity);
        }

        [Authorize]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var margin = _marginFormula.FirstOrDefault(x => x.ID == id);
            if (margin == null)
            {
                return NotFound();
            }

            return PartialView("_DeletePartial", margin);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, MarginFormulaViewModel entity)
        {
            var margin = _marginFormula.FirstOrDefault(x => x.ID == id);
            if (margin == null)
            {
                return NotFound();
            }

            try
            {
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting record: " + ex.Message);
            }

            return PartialView("_DeletePartial", margin);
        }
    }
}
