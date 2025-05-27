using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Filters;
using MVCWebApp.Helper;
using MVCWebApp.Models;
using MVCWebApp.Models.Req;
using MVCWebApp.Services;

namespace MVCWebApp.Controllers
{
    public class MarginController(
        IAuthService authService,
        IJwtTokenService jwtTokenService) : ControllerBase
    {
        private List<MarginViewModel> _margins = new List<MarginViewModel>
            {
                new MarginViewModel {  ID = 1, Username = "User1", Percentage = 10},
                new MarginViewModel {  ID = 2, Username = "User2", Percentage = 20},
                new MarginViewModel {  ID = 3, Username = "User3", Percentage = 30},
                new MarginViewModel {  ID = 4, Username = "User4", Percentage = 35},
                new MarginViewModel {  ID = 5, Username = "User5", Percentage = 40},
                new MarginViewModel {  ID = 6, Username = "User6", Percentage = 45},
                new MarginViewModel {  ID = 7, Username = "User7", Percentage = 50},
                new MarginViewModel {  ID = 8, Username = "User8", Percentage = 50},
                new MarginViewModel {  ID = 9, Username = "User9", Percentage = 60},
                new MarginViewModel {  ID = 10, Username = "User10", Percentage = 65},
                new MarginViewModel {  ID = 11, Username = "User11", Percentage = 67},
                new MarginViewModel {  ID = 12, Username = "User12", Percentage = 70},
                new MarginViewModel {  ID = 13, Username = "User13", Percentage = 80}
            };

        public async Task<IActionResult> Index(int? pageNumber, int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 10;  // Default page size is 10

            // Make the current page size accessible in the view
            ViewData["CurrentPageSize"] = currentPageSize;

            var paginatedStudents = PaginatedList<MarginViewModel>.CreateAsync(
                _margins,
                currentPage,
                currentPageSize);

            ViewBag.Username = Username;
            ViewBag.Email = Email;

            // Handle AJAX requests by returning only the results section
            //if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            //{
            //    return PartialView("_UsersTablePartial", paginatedStudents);
            //}

            //return PartialView("_Search", paginatedStudents);
            return View(new MarginSearchReq { PageNumber = 1, PageSize = 5 });
        }

        [HttpGet]
        public async Task<IActionResult> SearchMargins()
        {
            var paginatedStudents = PaginatedList<MarginViewModel>.CreateAsync(
               _margins,
               1,
               1);

            //return Json(paginatedStudents);
            return PartialView("_Search", paginatedStudents);
        }

        [HttpPost]
        public async Task<IActionResult> PostSearchMargins([FromBody] MarginSearchReq req)
        {
            int pageNumber = req.PageNumber ?? 1;
            int pageSize = req.PageSize ?? 10;

            var filterMargins = _margins
                .Where(x => 
                (string.IsNullOrEmpty(req.Username) || x.Username.Contains(req.Username, StringComparison.OrdinalIgnoreCase)) && 
                ((req.Percentage == null || req.Percentage == 0) || x.Percentage == req.Percentage));

            var paginatedMargins = PaginatedList<MarginViewModel>.CreateAsync(
                filterMargins,
                pageNumber,
                pageSize);

            return PartialView("_Search", paginatedMargins);
        }

        //[HttpGet]
        //public JsonResult GetMargins()
        //{
        //    _margins.Add(new MarginViewModel { ID = 3, Username = "User3", Percentage = 40 });
        //    return Json(_margins);
        //}

        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Create()
        {
            return PartialView("_CreatePartial", new MarginViewModel());
        }

        //[HttpPost]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(MarginViewModel entity)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var paginatedMargins = PaginatedList<MarginViewModel>.CreateAsync(
        //            _margins,
        //            1,
        //            1);

        //        return PartialView("_Search", paginatedMargins);
        //    }
        //    return PartialView("_CreatePartial", entity);
        //}

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MarginViewModel entity)
        {
            if (ModelState.IsValid)
            {
                return Json(new { success = true });
                //return RedirectToAction("Index", "Margin");
                //return Ok();
            }
            return PartialView("_CreatePartial", entity);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var entity = _margins.FirstOrDefault(x => x.ID == id);
            if (entity == null)
            {
                return NotFound();
            }
            return PartialView("_EditPartial", entity);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MarginViewModel entity)
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

            var margin = _margins.FirstOrDefault(x => x.ID == id);
            if (margin == null)
            {
                return NotFound();
            }

            return PartialView("_DetailPartial", margin);
        }

        [Authorize]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var margin = _margins.FirstOrDefault(x => x.ID == id);
            if (margin == null)
            {
                return NotFound();
            }

            return PartialView("_DeletePartial", margin);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, MarginViewModel entity)
        {
            var margin = _margins.FirstOrDefault(x => x.ID == id);
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
