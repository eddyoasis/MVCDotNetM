using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models;
using MVCWebApp.Models.MarginCalls;
using MVCWebApp.Repositories;
using MVCWebApp.ViewModels;

namespace MVCWebApp.Services
{
    public interface IMarginCallService
    {
        Task<bool> ApproveWithSP(MarginCallViewModel model);
        Task<MarginCallViewModel> GetByPortfolioID(string portfolioID);
        Task<MarginCall> GetEntityByPortfolioID(string portfolioID);
        Task<IEnumerable<string>> GetAllCollateralCcy();
        Task<IEnumerable<string>> GetAllVMCcy();
        Task<IEnumerable<string>> GetAllIMCcy();
        Task<IEnumerable<MarginCallViewModel>> GetAllAsync(MarginCallSearchReq req);
        Task<MarginCall?> GetByEntityIdAsync(int id);
        Task<MarginCallViewModel?> GetByIdAsync(int id);
        Task<bool> ApproveAsync(MarginCall entity);
        Task<bool> RejectAsync(MarginCall entity);
    }

    public class MarginCallService(
        IEmailService _emailService,
        IMarginCallRepository _marginCallRepository,
        ITaskQueueService _taskQueueService,
        IMapModel _mapper
        ) : BaseService, IMarginCallService
    {
        public async Task<bool> ApproveWithSP(MarginCallViewModel model)
        {
            var isSuccess = await _marginCallRepository.ApproveWithSP(model.PortfolioID);
            if (isSuccess)
            {
                var recipient = "eddy.wang@kgi.com";
                var subject = model.EmailTemplateSubject;
                var body = model.EmailTemplateValue;

                await _taskQueueService.AddSendEmailQueue(recipient, subject, body);
            }
            return isSuccess;
        }

        public async Task<MarginCallViewModel> GetByPortfolioID(string portfolioID)
        {
            var entity = await _marginCallRepository.GetByPortfolioID(portfolioID);

            return _mapper.MapDto<MarginCallViewModel>(entity);
        }

        public async Task<MarginCall> GetEntityByPortfolioID(string portfolioID)
        {
            return await _marginCallRepository.GetByPortfolioID(portfolioID);
        }

        public async Task<IEnumerable<string>> GetAllCollateralCcy() =>
            await _marginCallRepository.GetAllCollateralCcy();

        public async Task<IEnumerable<string>> GetAllVMCcy() =>
            await _marginCallRepository.GetAllVMCcy();

        public async Task<IEnumerable<string>> GetAllIMCcy() =>
            await _marginCallRepository.GetAllIMCcy();

        public async Task<IEnumerable<MarginCallViewModel>> GetAllAsync(MarginCallSearchReq req)
        {
            var marginCalls = _marginCallRepository.GetAllQueryable();

            marginCalls = marginCalls.Where(x =>
                //((req.DateFrom == DateTime.MinValue || req.DateTo == DateTime.MinValue) ||
                //    (req.IsSearchByCreatedDate ?
                //        x.InsertedDatetime >= req.DateFrom && x.InsertedDatetime < req.DateTo.AddDays(1) :
                //        x.ModifiedDatetime >= req.DateFrom && x.ModifiedDatetime < req.DateTo.AddDays(1))) &&
                (req.SearchByDateType == 1 ||
                     ((req.DateFrom == DateTime.MinValue || req.DateTo == DateTime.MinValue) ||
                            (req.SearchByDateType == 2 ?
                                    x.InsertedDatetime >= req.DateFrom && x.InsertedDatetime < req.DateTo.AddDays(1) :
                                    x.ModifiedDatetime >= req.DateFrom && x.ModifiedDatetime < req.DateTo.AddDays(1)))
                ) &&
                (req.PortfolioID.IsNullOrEmpty() || x.PortfolioID.ToLower().Contains(req.PortfolioID.ToLower())) &&
                (req.Percentages <= 0 || x.Percentages == req.Percentages) &&
                (req.Collateral <= 0 || x.Collateral == req.Collateral) &&
                (req.VM <= 0 || x.VM == req.VM) &&
                (req.IM <= 0 || x.VM == req.IM) &&
                (req.Selected_Collateral_Ccy == "All" || req.Selected_Collateral_Ccy == x.Collateral_Ccy) &&
                (req.Selected_IM_Ccy == "All" || req.Selected_IM_Ccy == x.IM_Ccy) &&
                (req.Selected_VM_Ccy == "All" || req.Selected_VM_Ccy == x.VM_Ccy) &&
                (req.SelectedMarginCallFlag == 0 ||
                    (req.SelectedMarginCallFlag == 1 && x.MarginCallFlag == "Y") ||
                    (req.SelectedMarginCallFlag == 2 && x.MarginCallFlag == "N")) &&
                (req.SelectedMTMTriggerFlag == 0 ||
                    (req.SelectedMTMTriggerFlag == 1 && x.MTMTriggerFlag == true) ||
                    (req.SelectedMTMTriggerFlag == 2 && (x.MTMTriggerFlag == null || x.MTMTriggerFlag == false))) &&
                (req.SelectedEODTriggerFlag == 0 ||
                    (req.SelectedEODTriggerFlag == 1 && x.EODTriggerFlag == true) ||
                    (req.SelectedEODTriggerFlag == 2 && (x.EODTriggerFlag == null || x.EODTriggerFlag == false))) &&
                (req.Type.IsNullOrEmpty() || x.Type.ToLower().Contains(req.Type.ToLower())) &&
                (req.Remarks.IsNullOrEmpty() || x.Remarks.ToLower().Contains(req.Remarks.ToLower()))
            );

            var searchReq = _mapper.MapDto<MarginCallSearchReq, BaseSearchReq>(req);

            var paginatedResult = await PaginatedList<MarginCallViewModel>.
                    GetByPagesAsync<MarginCall, MarginCallViewModel>(
                        marginCalls,
                        _mapper,
                        searchReq);

            return paginatedResult;
        }

        public async Task<MarginCall?> GetByEntityIdAsync(int id)
        {
            return await _marginCallRepository.GetByIdAsync(id);
        }

        public async Task<MarginCallViewModel?> GetByIdAsync(int id)
        {
            var entity = await _marginCallRepository.GetByIdAsync(id);

            return _mapper.MapDto<MarginCallViewModel>(entity);
        }

        public async Task<bool> ApproveAsync(MarginCall entity)
        {
            try
            {
                entity.MarginCallFlag = "Y";

                _mapper.Map(entity, Username);

                await _marginCallRepository.UpdateAsync(entity);


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public async Task<bool> RejectAsync(MarginCall entity)
        {
            try
            {
                //entity.Status = (int)MarginCallSearchStatusEnum.Rejected;

                //_mapper.Map(entity, Username);

                //await _marginCallRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
    }
}
