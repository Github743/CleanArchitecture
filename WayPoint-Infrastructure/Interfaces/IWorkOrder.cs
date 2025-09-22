using WayPoint.Model;
using WayPoint.Model.Enhanced;
using WayPoint.Model.ViewModels;

namespace WayPoint_Infrastructure.Interfaces
{
    public interface IWorkOrder
    {
        Task<WorkOrder> CreateWorkOrder(WorkOrderCreationViewModel workOrderCreationViewModel, CancellationToken ct = default);
        Task<WorkOrderCreationViewModel> GetSystemWorkOrderFlowData(WorkOrderCreationViewModel workOrderCreationViewModel, CancellationToken ct = default);
        Task<PagedResult<WorkOrderDetail>> SearchAsync(WorkOrderSearchRequest req, CancellationToken ct = default);
        Task<BFADetailsViewModel> GetBFAOptionsDetailsAsync(int workOrderId, CancellationToken ct = default);
        Task<bool> SaveOptionsAsync(BFADetailsViewModel workOrderSettingField, int workOrderId,
            CancellationToken ct = default);
        Task<IReadOnlyList<WorkOrder>> GetPendingWorkOrdersbyContext(int? Filter, int? systemworkorderid, bool isClientContext, CancellationToken ct = default);
        Task<List<SystemWorkOrderGroup>> GetSystemWorkOrderGroup(int systemWorkOrderId, CancellationToken ct = default);
    }
}
