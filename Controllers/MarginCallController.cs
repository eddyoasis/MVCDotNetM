using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCWebApp.BackgroundServices;
using MVCWebApp.Enums;
using MVCWebApp.Helper;
using MVCWebApp.Models;
using MVCWebApp.Models.Req;

namespace MVCWebApp.Controllers
{
    public class MarginCallController(IBackgroundTaskQueue taskQueue) : ControllerBase
    {

        private List<MarginCallViewModel> _marginCall =
            [
                new MarginCallViewModel
                {
                    ID = 1,
                    CcyCode = "SGD",
                    ClientCode = "ClientCode1",
                    IM = "IM2",
                    LedgerBal = "LedgerBal2",
                    OrderDetails = "OrderDetails2",
                    Percentages = 10,
                    Status = "Pending",
                    TimeStemp = DateTime.Now,
                    TNE = "TNE2",
                    TypeOfMarginCall = "TypeOfMarginCall2"
                },
                new MarginCallViewModel
                    {
                        ID = 2,
                        CcyCode = "SGD",
                        ClientCode = "ClientCode2",
                        IM = "IM2",
                        LedgerBal = "LedgerBal2",
                        OrderDetails = "OrderDetails2",
                        Percentages = 10,
                        Status = "Approved",
                        TimeStemp = DateTime.Now,
                        TNE = "TNE2",
                        TypeOfMarginCall = "TypeOfMarginCall2"
                    },
                new MarginCallViewModel
                    {
                        ID = 3,
                        CcyCode = "SGD",
                        ClientCode = "ClientCode3",
                        IM = "IM3",
                        LedgerBal = "LedgerBal3",
                        OrderDetails = "OrderDetails3",
                        Percentages = 10,
                        Status = "Rejected",
                        TimeStemp = DateTime.Now,
                        TNE = "TNE3",
                        TypeOfMarginCall = "TypeOfMarginCall3"
                    }
            ];

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
            ViewBag.Username = Username;

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
            int pageNumber = req.PageNumber ?? 1;
            int pageSize = req.PageSize ?? 10;

            var paginatedResult = PaginatedList<MarginCallViewModel>.CreateAsync(
                _marginCall,
                pageNumber,
                pageSize);

            return PartialView("_Search", paginatedResult);
        }

        [Authorize]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _marginCall.FirstOrDefault(x => x.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_DetailPartial", entity);
        }

        [Authorize]
        public IActionResult Approve(int id)
        {
            var entity = _marginCall.FirstOrDefault(x => x.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_ApprovePartial", entity);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(MarginCallViewModel entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error approving record: " + ex.Message);
                }
            }
            return PartialView("_ApprovePartial", entity);
        }

        [Authorize]
        public IActionResult Reject(int id)
        {
            var entity = _marginCall.FirstOrDefault(x => x.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            return PartialView("_RejectPartial", entity);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(MarginCallViewModel entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error rejecting record: " + ex.Message);
                }
            }
            return PartialView("_RejectPartial", entity);
        }
    }
}
