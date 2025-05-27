using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Helper;
using MVCWebApp.Models.Req;
using MVCWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace MVCWebApp.Controllers
{
    public class EmailNotificationController : ControllerBase
    {
        private List<EmailNotificationViewModel> _emailNotifications = new List<EmailNotificationViewModel>
            {
                new EmailNotificationViewModel
                {
                    ID = 1,
                    MarginType = "MarginType1",
                    EmailTemplate = "EmailTemplate1",
                     CreatedAt = DateTime.Now.AddDays(-1),
                      CreatedBy = "Admin123",
                },
                new EmailNotificationViewModel
                {
                    ID = 2,
                    MarginType = "MarginType2",
                    EmailTemplate = "EmailTemplate2",
                     CreatedAt = DateTime.Now.AddDays(-1),
                      CreatedBy = "Admin123",
                },
                new EmailNotificationViewModel
                {
                    ID = 3,
                    MarginType = "MarginType3",
                    EmailTemplate = "EmailTemplate3",
                     CreatedAt = DateTime.Now.AddDays(-1),
                      CreatedBy = "Admin123",
                       ModifiedAt = DateTime.Now,
                        ModifiedBy = "Admin123"
                },
                new EmailNotificationViewModel
                {
                    ID = 4,
                    MarginType = "MarginType4",
                    EmailTemplate = "EmailTemplate4",
                     CreatedAt = DateTime.Now.AddDays(-1),
                      CreatedBy = "Admin123",
                },
                new EmailNotificationViewModel
                {
                    ID = 5,
                    MarginType = "MarginType5",
                    EmailTemplate = "EmailTemplate5",
                     CreatedAt = DateTime.Now.AddDays(-1),
                      CreatedBy = "Admin123",
                },
                new EmailNotificationViewModel
                {
                    ID = 6,
                    MarginType = "MarginType6",
                    EmailTemplate = "EmailTemplate6",
                     CreatedAt = DateTime.Now.AddDays(-1),
                      CreatedBy = "Admin123",
                       ModifiedAt = DateTime.Now,
                        ModifiedBy = "Admin123"
                },
                new EmailNotificationViewModel
                {
                    ID = 7,
                    MarginType = "MarginType7",
                    EmailTemplate = "EmailTemplate7",
                     CreatedAt = DateTime.Now.AddDays(-1),
                      CreatedBy = "Admin123",
                       ModifiedAt = DateTime.Now,
                        ModifiedBy = "Admin123"
                }
            };

        public async Task<IActionResult> Index()
        {
            ViewBag.Username = Username;

            return View(new EmailNotificationSearchReq { PageNumber = 1, PageSize = 5 });
        }

        [HttpPost]
        public async Task<IActionResult> SearchEmailNotifications([FromBody] EmailNotificationSearchReq req)
        {
            int pageNumber = req.PageNumber ?? 1;
            int pageSize = req.PageSize ?? 10;

            var paginatedResult = PaginatedList<EmailNotificationViewModel>.CreateAsync(
                _emailNotifications,
                pageNumber,
                pageSize);

            return PartialView("_Search", paginatedResult);
        }

        public IActionResult Create()
        {
            return PartialView("_CreatePartial", new EmailNotificationAddReq());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmailNotificationAddReq entity)
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
            var entity = _emailNotifications.FirstOrDefault(x => x.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            var res = new EmailNotificationEditReq
            {
                ID = entity.ID,
                MarginType = entity.MarginType,
                EmailTemplate = entity.EmailTemplate
            };

            return PartialView("_EditPartial", res);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmailNotificationEditReq entity)
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

            var entity = _emailNotifications.FirstOrDefault(x => x.ID == id);
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

            var margin = _emailNotifications.FirstOrDefault(x => x.ID == id);
            if (margin == null)
            {
                return NotFound();
            }

            return PartialView("_DeletePartial", margin);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, EmailNotificationViewModel entity)
        {
            var margin = _emailNotifications.FirstOrDefault(x => x.ID == id);
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
