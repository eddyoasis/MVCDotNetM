using MVCWebApp.Enums;
using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models;
using MVCWebApp.Models.MarginCalls;
using MVCWebApp.Repositories;
using MVCWebApp.ViewModels;
using System.Linq;

namespace MVCWebApp.Services
{
    public interface IMarginCallService
    {
        ClientEmailDBResult GetClientEmail(string portfolioID);

        Task<IEnumerable<string>> GetAllCollateralCcy();
        Task<IEnumerable<string>> GetAllVMCcy();
        Task<IEnumerable<string>> GetAllIMCcy();

        Task<IEnumerable<string>> GetAllCollateralCcyMTM();
        Task<IEnumerable<string>> GetAllVMCcyMTM();
        Task<IEnumerable<string>> GetAllIMCcyMTM();

        Task<bool> ApproveMarginCallEOD(MarginCallViewModel model);
        MarginCallViewModel GetMarginCallEOD(string portfolioID);
        IEnumerable<MarginCallViewModel> GetMarginCallEODAll(MarginCallSearchReq req);


        Task<bool> ApproveMarginCallMTM(MarginCallViewModel model);
        Task<bool> ApproveMarginCallMTMStockloss(MarginCallViewModel model);
        MarginCallViewModel GetMarginCallMTM(string portfolioID);
        IEnumerable<MarginCallViewModel> GetMarginCallMTMAll(MarginCallSearchReq req);
    }

    public class MarginCallService(
        IEmailService _emailService,
        IMarginCallRepository _marginCallRepository,
        ITaskQueueService _taskQueueService,
        IMapModel _mapper
        ) : BaseService, IMarginCallService
    {
        public MarginCallViewModel GetMarginCallMTM(string portfolioID)
        {
            var entity = _marginCallRepository.GetMarginCallMTM(portfolioID);

            return _mapper.MapDto<MarginCallViewModel>(entity);
        }

        public ClientEmailDBResult GetClientEmail(string portfolioID) =>
             _marginCallRepository.GetClientEmail(portfolioID);

        public async Task<IEnumerable<string>> GetAllCollateralCcyMTM() =>
            await _marginCallRepository.GetAllCollateralCcyMTM();

        public async Task<IEnumerable<string>> GetAllVMCcyMTM() =>
            await _marginCallRepository.GetAllVMCcyMTM();

        public async Task<IEnumerable<string>> GetAllIMCcyMTM() =>
            await _marginCallRepository.GetAllIMCcyMTM();

        public async Task<IEnumerable<string>> GetAllCollateralCcy() =>
            await _marginCallRepository.GetAllCollateralCcy();

        public async Task<IEnumerable<string>> GetAllVMCcy() =>
            await _marginCallRepository.GetAllVMCcy();

        public async Task<IEnumerable<string>> GetAllIMCcy() =>
            await _marginCallRepository.GetAllIMCcy();



        /*-------------------------------------------------    EOD     ----------*/
        public async Task<bool> ApproveMarginCallEOD(MarginCallViewModel model)
        {
            var isSuccess = await _marginCallRepository.ApproveMarginCallEOD(model.PortfolioID);
            if (isSuccess)
            {
                var emailTo = model.EmailTo.Split("\r\n");
                var emailCC = model.EmailCC?.Split("\r\n");

                List<string> recipientsTo = emailTo.ToList();
                List<string> recipientsCC = emailCC == null ? new List<string>() : emailCC.ToList();
                var subject = model.EmailTemplateSubject;
                var body = model.EmailTemplateValue;

                await _taskQueueService.AddSendEmailQueue(recipientsTo, recipientsCC, subject, body);
            }
            return isSuccess;
        }

        public MarginCallViewModel GetMarginCallEOD(string portfolioID)
        {
            var entity = _marginCallRepository.GetMarginCallEOD(portfolioID);

            return _mapper.MapDto<MarginCallViewModel>(entity);
        }

        public IEnumerable<MarginCallViewModel> GetMarginCallEODAll(MarginCallSearchReq req)
        {
            var marginCalls = _marginCallRepository.GetMarginCallEOD(
                req.Selected_MarginMode == (int)MarginCallMode.TriggeredToday ? MarginCallMode.TriggeredToday : MarginCallMode.All);

            marginCalls = marginCalls.Where(x =>
                ((req.Selected_MarginMode == (int)MarginCallMode.All || req.Selected_MarginMode == (int)MarginCallMode.TriggeredToday) ||
                    (req.Selected_MarginMode == (int)MarginCallMode.MarginAvailable && !x.MarginCallTriggerFlag) ||
                    (req.Selected_MarginMode == (int)MarginCallMode.StoplossAvailable && (x.Day != "1" && !x.StoplossTriggerFlag)) ||
                    (req.Selected_MarginMode == (int)MarginCallMode.MOCAvailable && (x.Day == "3" && !x.MOCTriggerFlag))
                ) &&
                (req.SearchByDateType == 1 ||
                     ((req.DateFrom == DateTime.MinValue || req.DateTo == DateTime.MinValue) ||
                            (req.SearchByDateType == 2 ?
                                    x.InsertedDatetime >= req.DateFrom && x.InsertedDatetime < req.DateTo.AddDays(1) :
                                    x.ModifiedDatetime >= req.DateFrom && x.ModifiedDatetime < req.DateTo.AddDays(1)))
                ) &&
                (req.PortfolioID.IsNullOrEmpty() || x.PortfolioID.ToLower().Contains(req.PortfolioID.ToLower())) &&
                ((req.PercentagesFrom <= 0 && req.PercentagesTo <= 0) ||
                    ((req.PercentagesFrom > 0 && req.PercentagesTo <= 0) && x.Percentages >= req.PercentagesFrom) ||
                    ((req.PercentagesFrom <= 0 && req.PercentagesTo > 0) && x.Percentages <= req.PercentagesTo) ||
                    (x.Percentages >= req.PercentagesFrom && x.Percentages <= req.PercentagesTo)) &&
                ((req.CollateralFrom <= 0 && req.CollateralTo <= 0) ||
                    ((req.CollateralFrom > 0 && req.CollateralTo <= 0) && x.Collateral >= req.CollateralFrom) ||
                    ((req.CollateralFrom <= 0 && req.CollateralTo > 0) && x.Collateral <= req.CollateralTo) ||
                    (x.Collateral >= req.CollateralFrom && x.Collateral <= req.CollateralTo)) &&
                ((req.VMFrom <= 0 && req.VMTo <= 0) ||
                    ((req.VMFrom > 0 && req.VMTo <= 0) && x.VM >= req.VMFrom) ||
                    ((req.VMFrom <= 0 && req.VMTo > 0) && x.VM <= req.VMTo) ||
                    (x.VM >= req.VMFrom && x.VM <= req.VMTo)) &&
                ((req.IMFrom <= 0 && req.IMTo <= 0) ||
                    ((req.IMFrom > 0 && req.IMTo <= 0) && x.IM >= req.IMFrom) ||
                    ((req.IMFrom <= 0 && req.IMTo > 0) && x.IM <= req.IMTo) ||
                    (x.IM >= req.IMFrom && x.IM <= req.IMTo)) &&
                (req.Selected_Collateral_Ccy == "All" || req.Selected_Collateral_Ccy == x.Collateral_Ccy) &&
                (req.Selected_IM_Ccy == "All" || req.Selected_IM_Ccy == x.IM_Ccy) &&
                (req.Selected_VM_Ccy == "All" || req.Selected_VM_Ccy == x.VM_Ccy)
            );

            if (marginCalls.Any())
            {
                Func<MarginCallDto, object> orderSelector = x =>
                    req.Selected_MarginCallOrderByColumn switch
                    {
                        1 => x.PortfolioID,
                        2 => x.Percentages,
                        3 => x.MarginCallAmount,
                        4 => x.Collateral,
                        5 => x.VM,
                        6 => x.IM,
                        7 => x.InsertedDatetime,
                        _ => x.ModifiedDatetime
                    };

                marginCalls = req.SearchByOrderByType == 1
                ? marginCalls.OrderBy(orderSelector)
                : marginCalls.OrderByDescending(orderSelector);
            }

            var searchReq = _mapper.MapDto<MarginCallSearchReq, BaseSearchReq>(req);

            var paginatedResult = PaginatedList<MarginCallViewModel>.
                    GetByPagesIEnumerable<MarginCallDto, MarginCallViewModel>(
                        marginCalls,
                        _mapper,
                        searchReq);

            return paginatedResult;
        }

        /*-------------------------------------------------    MTM     ----------*/
        public async Task<bool> ApproveMarginCallMTM(MarginCallViewModel model)
        {
            var isSuccess = await _marginCallRepository.ApproveMarginCallMTM(model.PortfolioID);
            if (isSuccess)
            {
                var emailTo = model.EmailTo.Split("\r\n");
                var emailCC = model.EmailCC?.Split("\r\n");

                List<string> recipientsTo = emailTo.ToList();
                List<string> recipientsCC = emailCC == null ? new List<string>() : emailCC.ToList();
                var subject = model.EmailTemplateSubject;
                var body = model.EmailTemplateValue;

                await _taskQueueService.AddSendEmailQueue(recipientsTo, recipientsCC, subject, body);
            }
            return isSuccess;
        }

        public async Task<bool> ApproveMarginCallMTMStockloss(MarginCallViewModel model)
        {
            var isSuccess = await _marginCallRepository.ApproveMarginCallMTM(model.PortfolioID);
            if (isSuccess)
            {
                var emailTo = model.EmailTo.Split("\r\n");
                var emailCC = model.EmailCC?.Split("\r\n");

                List<string> recipientsTo = emailTo.ToList();
                List<string> recipientsCC = emailCC == null ? new List<string>(): emailCC.ToList();
                var subject = model.EmailTemplateSubject;
                var body = model.EmailTemplateValue;

                await _taskQueueService.AddSendEmailQueue(recipientsTo, recipientsCC, subject, body);
            }
            return isSuccess;
        }

        public IEnumerable<MarginCallViewModel> GetMarginCallMTMAll(MarginCallSearchReq req)
        {
            var marginCalls = _marginCallRepository.GetMarginCallMTM(
                req.Selected_MarginMode == (int)MarginCallMode.TriggeredToday ? MarginCallMode.TriggeredToday : MarginCallMode.All);

            marginCalls = marginCalls.Where(x =>
                ((req.Selected_MarginMode == (int)MarginCallMode.All || req.Selected_MarginMode == (int)MarginCallMode.TriggeredToday) ||
                    (req.Selected_MarginMode == (int)MarginCallMode.MarginAvailable && !x.MarginCallTriggerFlag) ||
                    (req.Selected_MarginMode == (int)MarginCallMode.StoplossAvailable && !x.StoplossTriggerFlag)
                ) && 
                (req.SearchByDateType == 1 ||
                     ((req.DateFrom == DateTime.MinValue || req.DateTo == DateTime.MinValue) ||
                            (req.SearchByDateType == 2 ?
                                    x.InsertedDatetime >= req.DateFrom && x.InsertedDatetime < req.DateTo.AddDays(1) :
                                    x.ModifiedDatetime >= req.DateFrom && x.ModifiedDatetime < req.DateTo.AddDays(1)))
                ) &&
                (req.PortfolioID.IsNullOrEmpty() || x.PortfolioID.ToLower().Contains(req.PortfolioID.ToLower())) &&
                ((req.PercentagesFrom <= 0 && req.PercentagesTo <= 0) ||
                    ((req.PercentagesFrom > 0 && req.PercentagesTo <= 0) && x.Percentages >= req.PercentagesFrom) ||
                    ((req.PercentagesFrom <= 0 && req.PercentagesTo > 0) && x.Percentages <= req.PercentagesTo) ||
                    (x.Percentages >= req.PercentagesFrom && x.Percentages <= req.PercentagesTo)) &&
                ((req.CollateralFrom <= 0 && req.CollateralTo <= 0) ||
                    ((req.CollateralFrom > 0 && req.CollateralTo <= 0) && x.Collateral >= req.CollateralFrom) ||
                    ((req.CollateralFrom <= 0 && req.CollateralTo > 0) && x.Collateral <= req.CollateralTo) ||
                    (x.Collateral >= req.CollateralFrom && x.Collateral <= req.CollateralTo)) &&
                ((req.VMFrom <= 0 && req.VMTo <= 0) ||
                    ((req.VMFrom > 0 && req.VMTo <= 0) && x.VM >= req.VMFrom) ||
                    ((req.VMFrom <= 0 && req.VMTo > 0) && x.VM <= req.VMTo) ||
                    (x.VM >= req.VMFrom && x.VM <= req.VMTo)) &&
                ((req.IMFrom <= 0 && req.IMTo <= 0) ||
                    ((req.IMFrom > 0 && req.IMTo <= 0) && x.IM >= req.IMFrom) ||
                    ((req.IMFrom <= 0 && req.IMTo > 0) && x.IM <= req.IMTo) ||
                    (x.IM >= req.IMFrom && x.IM <= req.IMTo)) &&
                (req.Selected_Collateral_Ccy == "All" || req.Selected_Collateral_Ccy == x.Collateral_Ccy) &&
                (req.Selected_IM_Ccy == "All" || req.Selected_IM_Ccy == x.IM_Ccy) &&
                (req.Selected_VM_Ccy == "All" || req.Selected_VM_Ccy == x.VM_Ccy)
            //(req.Type.IsNullOrEmpty() || x.Type.ToLower().Contains(req.Type.ToLower())) &&
            //(req.Remarks.IsNullOrEmpty() || x.Remarks.ToLower().Contains(req.Remarks.ToLower()))
            );

            if (marginCalls.Any())
            {
                Func<MarginCallDto, object> orderSelector = x =>
                    req.Selected_MarginCallOrderByColumn switch
                    {
                        1 => x.PortfolioID,
                        2 => x.Percentages,
                        3 => x.MarginCallAmount,
                        4 => x.Collateral,
                        5 => x.VM,
                        6 => x.IM,
                        7 => x.InsertedDatetime,
                        _ => x.ModifiedDatetime
                    };

                marginCalls = req.SearchByOrderByType == 1
                ? marginCalls.OrderBy(orderSelector)
                : marginCalls.OrderByDescending(orderSelector);
            }

            var searchReq = _mapper.MapDto<MarginCallSearchReq, BaseSearchReq>(req);

            var paginatedResult = PaginatedList<MarginCallViewModel>.
                    GetByPagesIEnumerable<MarginCallDto, MarginCallViewModel>(
                        marginCalls,
                        _mapper,
                        searchReq);

            return paginatedResult;
        }

        //public async Task<IEnumerable<MarginCallViewModel>> GetAllAsync2(MarginCallSearchReq req)
        //{
        //    var marginCalls = _marginCallRepository.GetAllQueryable();

        //    marginCalls = marginCalls.Where(x =>
        //        //((req.DateFrom == DateTime.MinValue || req.DateTo == DateTime.MinValue) ||
        //        //    (req.IsSearchByCreatedDate ?
        //        //        x.InsertedDatetime >= req.DateFrom && x.InsertedDatetime < req.DateTo.AddDays(1) :
        //        //        x.ModifiedDatetime >= req.DateFrom && x.ModifiedDatetime < req.DateTo.AddDays(1))) &&
        //        (req.SearchByDateType == 1 ||
        //             ((req.DateFrom == DateTime.MinValue || req.DateTo == DateTime.MinValue) ||
        //                    (req.SearchByDateType == 2 ?
        //                            x.InsertedDatetime >= req.DateFrom && x.InsertedDatetime < req.DateTo.AddDays(1) :
        //                            x.ModifiedDatetime >= req.DateFrom && x.ModifiedDatetime < req.DateTo.AddDays(1)))
        //        ) &&
        //        (req.PortfolioID.IsNullOrEmpty() || x.PortfolioID.ToLower().Contains(req.PortfolioID.ToLower())) &&
        //        //(req.Percentages <= 0 || x.Percentages == req.Percentages) &&
        //        //(req.Collateral <= 0 || x.Collateral == req.Collateral) &&
        //        //(req.VM <= 0 || x.VM == req.VM) &&
        //        //(req.IM <= 0 || x.VM == req.IM) &&
        //        (req.Selected_Collateral_Ccy == "All" || req.Selected_Collateral_Ccy == x.Collateral_Ccy) &&
        //        (req.Selected_IM_Ccy == "All" || req.Selected_IM_Ccy == x.IM_Ccy) &&
        //        (req.Selected_VM_Ccy == "All" || req.Selected_VM_Ccy == x.VM_Ccy) &&
        //        (req.SelectedMarginCallFlag == 0 ||
        //            (req.SelectedMarginCallFlag == 1 && x.MarginCallFlag == "Y") ||
        //            (req.SelectedMarginCallFlag == 2 && x.MarginCallFlag == "N")) &&
        //        (req.SelectedMTMTriggerFlag == 0 ||
        //            (req.SelectedMTMTriggerFlag == 1 && x.MTMTriggerFlag == true) ||
        //            (req.SelectedMTMTriggerFlag == 2 && (x.MTMTriggerFlag == null || x.MTMTriggerFlag == false))) &&
        //        (req.SelectedEODTriggerFlag == 0 ||
        //            (req.SelectedEODTriggerFlag == 1 && x.EODTriggerFlag == true) ||
        //            (req.SelectedEODTriggerFlag == 2 && (x.EODTriggerFlag == null || x.EODTriggerFlag == false))) &&
        //        (req.Type.IsNullOrEmpty() || x.Type.ToLower().Contains(req.Type.ToLower())) &&
        //        (req.Remarks.IsNullOrEmpty() || x.Remarks.ToLower().Contains(req.Remarks.ToLower()))
        //    );

        //    var searchReq = _mapper.MapDto<MarginCallSearchReq, BaseSearchReq>(req);

        //    var paginatedResult = await PaginatedList<MarginCallViewModel>.
        //            GetByPagesAsync<MarginCall, MarginCallViewModel>(
        //                marginCalls,
        //                _mapper,
        //                searchReq);

        //    return paginatedResult;
        //}

    }
}
