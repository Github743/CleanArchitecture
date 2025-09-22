using WayPoint.Model;

namespace WayPoint_Infrastructure.Interfaces
{
    public interface IClientAgreementRepository
    {
        Task<bool> SaveEntityProducts(List<WorkOrderClientAgreementEntityProduct> workOrderClientAgreementEntityProducts, int workOrderId, CancellationToken ct = default);

        Task<IReadOnlyList<WorkOrderClientAgreementEntityProduct>> GetWorkOrderClientAgreementEntityProducts(int workOrderId, CancellationToken ct = default);

        Task<WorkOrderClientAgreement> GetWorkOrderClientAgreement(int workOrderId, CancellationToken ct = default);
        Task<IReadOnlyList<WorkOrderClientAgreementEntityProduct>> GetAdditionalDiscountedProducts(int workOrderId, CancellationToken ct = default);
    }
}
