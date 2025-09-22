﻿using System.Collections.ObjectModel;
using WayPoint.Model;
using WayPoint_Infrastructure.Data;
using WayPoint_Infrastructure.Interfaces;

namespace WayPoint_Infrastructure.Repositories
{
    public class ClientAgreementRepository(ISqlEngine sql, IEfReadEngine<WayPointDbContext> ef) : IClientAgreementRepository
    {
        private readonly ISqlEngine _sql = sql ?? throw new ArgumentNullException(nameof(sql));
        private readonly IEfReadEngine<WayPointDbContext> _ef = ef;

        public async Task<bool> SaveEntityProducts(
            List<WorkOrderClientAgreementEntityProduct> workOrderClientAgreementEntityProducts,
            int workOrderId, CancellationToken ct = default)
        {
            try
            {
                var workOrderClientAgreement = await _sql.RetrieveObjectAsync<WorkOrderClientAgreement>(
                new { WorkOrderId = workOrderId },
                ct);
                var now = DateTime.UtcNow;

                var existingList = await _ef.RetrieveAsync<WorkOrderClientAgreementEntityProduct>(
                    e => e.WorkOrderClientAgreementId == workOrderClientAgreement.WorkOrderClientAgreementId,
                    ct: ct);

                var existingById = existingList
                    .Where(x => x.WorkOrderClientAgreementEntityProductId != 0)
                    .ToDictionary(x => x.WorkOrderClientAgreementEntityProductId);

                foreach (var e in workOrderClientAgreementEntityProducts)
                {
                    e.WorkOrderClientAgreementId = workOrderClientAgreement.WorkOrderClientAgreementId;
                    e.LastUpdateDate = now;
                    e.LastUpdatedBy = "dmeka";
                    if (e.WorkOrderClientAgreementEntityProductId == 0)
                    {
                        e.CreationDate = now;
                        e.CreatedBy = "dmeka";
                    }
                }
                await _ef.SaveEntities(workOrderClientAgreementEntityProducts,
                    useTransaction: false, ct: ct, bulkOperation: null);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IReadOnlyList<WorkOrderClientAgreementEntityProduct>> GetWorkOrderClientAgreementEntityProducts(
            int workOrderId, CancellationToken ct = default)
        {
            var workOrderClientAgreement = await _sql.RetrieveObjectAsync<WorkOrderClientAgreement>(new { workOrderId }, ct);
            var queryable = await _sql.RetrieveObjectsAsync<WorkOrderClientAgreementEntityProduct>(
                new { workOrderClientAgreement.WorkOrderClientAgreementId },
                ct
            );

            var list = queryable
                        .Where(x => x.IsAdditionalDiscount == false
                        && x.WorkOrderClientAgreementEntityId == null)
                        .OrderBy(m => m.TonnageBilling)
                        .ThenBy(m => m.DefaultOrder)
                        .ToList();

            return new ReadOnlyCollection<WorkOrderClientAgreementEntityProduct>(list);
        }

        public async Task<WorkOrderClientAgreement> GetWorkOrderClientAgreement(int workOrderId, CancellationToken ct = default)
        {
            return await _sql.RetrieveObjectAsync<WorkOrderClientAgreement>(new { workOrderId }, ct);
        }

        public async Task<IReadOnlyList<WorkOrderClientAgreementEntityProduct>> GetAdditionalDiscountedProducts(int workOrderId, CancellationToken ct = default)
        {
            var workOrderClientAgreement = await _sql.RetrieveObjectAsync<WorkOrderClientAgreement>(new { workOrderId }, ct);
            var queryable = await _sql.RetrieveObjectsAsync<WorkOrderClientAgreementEntityProduct>(
                new { workOrderClientAgreement.WorkOrderClientAgreementId },
                ct
            );

            var list = queryable
                        .Where(x => x.IsAdditionalDiscount == true
                        && x.WorkOrderClientAgreementEntityId == null)
                        .OrderByDescending(a => a.Amount)
                        .ToList();

            return new ReadOnlyCollection<WorkOrderClientAgreementEntityProduct>(list);
        }

        public async Task<bool> UpdateEntityProduct(WorkOrderClientAgreementEntityProduct workOrderClientAgreementEntityProduct, CancellationToken ct = default)
        {
            try
            {
                var agreementEntityProduct = await _sql.RetrieveObjectAsync<WorkOrderClientAgreementEntityProduct>(new { workOrderClientAgreementEntityProduct.WorkOrderClientAgreementEntityProductId }, ct);
                if (agreementEntityProduct == null || agreementEntityProduct.WorkOrderClientAgreementEntityProductId == 0)
                    return false;

                agreementEntityProduct.SystemProductId = workOrderClientAgreementEntityProduct.SystemProductId;
                agreementEntityProduct.DiscountType = workOrderClientAgreementEntityProduct.DiscountType;
                agreementEntityProduct.Amount = workOrderClientAgreementEntityProduct.Amount;
                agreementEntityProduct.LimitPerYear = workOrderClientAgreementEntityProduct.LimitPerYear;
                agreementEntityProduct.ExpiryDate = workOrderClientAgreementEntityProduct.ExpiryDate;
                agreementEntityProduct.LastUpdatedBy = "dmeka";
                agreementEntityProduct.LastUpdateDate = DateTime.Now;


                await _ef.SaveEntity(agreementEntityProduct,
                    ct: ct);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveEntityProduct(int workOrderClientAgreementEntityProductId, CancellationToken ct = default)
        {
            try
            {
                var agreementEntityProduct = await _sql.RetrieveObjectAsync<WorkOrderClientAgreementEntityProduct>(new { workOrderClientAgreementEntityProductId }, ct);
                if (agreementEntityProduct == null || agreementEntityProduct.WorkOrderClientAgreementEntityProductId == 0)
                    return false;

                agreementEntityProduct.LastUpdatedBy = "dmeka";
                agreementEntityProduct.LastUpdateDate = DateTime.Now;
                agreementEntityProduct.Removed = true;
                await _ef.SaveEntity(agreementEntityProduct,
                    ct: ct);
                return true;
            }
            catch (Exception ex) { return false; }

        }
    }
}
