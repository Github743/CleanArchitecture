using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WayPoint.Model;
using WayPoint.Model.Enhanced;
using WayPoint.Model.Helper;
using WayPoint.Model.ViewModels;
using WayPoint_Infrastructure.Data;
using WayPoint_Infrastructure.Helpers;
using WayPoint_Infrastructure.Interfaces;

namespace WayPoint_Infrastructure.Repositories
{
    public class WorkorderRepository(ISqlEngine sql, IEfReadEngine<WayPointDbContext> ef,
        DbContext db) : IWorkOrder
    {
        private readonly ISqlEngine _sql = sql;
        private readonly IEfReadEngine<WayPointDbContext> _ef = ef;
        private readonly DbContext _db = db;
        public async Task<WorkOrder> CreateWorkOrder(WorkOrderCreationViewModel workOrderCreationViewModel, CancellationToken ct = default)
        {
            WorkOrder workOrder = new();
            try
            {
                workOrderCreationViewModel = await GetSystemWorkOrderFlowData(workOrderCreationViewModel, ct);

                SystemCreationData systemCreationData = await GetSystemCreationData(workOrderCreationViewModel, ct);

                List<Client> lClient = [];

                foreach (var entities in workOrderCreationViewModel.Entities)
                {
                    if (entities.ClientId.HasValue)
                    {
                        if (workOrderCreationViewModel.systemWorkOrder.ClientContext)
                        {
                            Client client = await _sql.RetrieveObjectAsync<Client>(
                                new { entities.ClientId });
                            lClient.Add(client);
                        }
                    }
                }

                Entity entity = await GetEntityData(ct);

                List<WorkOrderEntity> lWorkOrderEntity = null;


                await _sql.WithDbTransactionAsync(async (conn, tran) =>
                {
                    var newWorkOrderEntity = await AddEntityAsync(entity, tran, ct);
                    workOrder = await AddWorkOrder(workOrderCreationViewModel, newWorkOrderEntity, tran, ct);
                    lWorkOrderEntity = await CreateWorkOrderEntities(workOrder, lClient, tran, ct);
                    await CreateWorkOrderItemEntities(systemCreationData, workOrderCreationViewModel, workOrder, lWorkOrderEntity, tran, ct);
                    await AddWOHistory(workOrderCreationViewModel, workOrder, tran, ct);
                });
                await SaveClientAgreement(workOrder.WorkOrderId, ct);
            }
            catch (Exception ex)
            {

            }
            return workOrder;
        }

        public async Task<WorkOrderCreationViewModel> GetSystemWorkOrderFlowData(WorkOrderCreationViewModel workOrderCreationViewModel, CancellationToken ct = default)
        {
            //WorkOrderCreationViewModel workOrderCreationViewModel = new();

            var systemWorkOrder = await _sql.RetrieveObjectAsync<SystemWorkOrder>(
                new { SystemWorkOrderName = ApplicationConstants.ISSUE_BFA_WORKORDER }, ct);

            if (systemWorkOrder == null)
                return workOrderCreationViewModel;

            var workOrderTypeFlows = await _sql.RetrieveObjectsAsync<SystemWorkOrderTypeFlow>(
                new { systemWorkOrder.SystemWorkOrderId },
                ct);

            var systemWorkOrderTypeFlow = workOrderTypeFlows.FirstOrDefault(f => f.IsInternal);
            if (systemWorkOrderTypeFlow == null)
                return workOrderCreationViewModel;

            var workOrderFlowSteps = await _sql.RetrieveObjectsAsync<SystemWorkOrderFlowStep>(
                new { systemWorkOrderTypeFlow.SystemWorkOrderFlowId },
                ct);

            SystemWorkOrderFlowStep firstSystemWorkOrderFlowStep = workOrderFlowSteps.FirstOrDefault(s => s.CurrentStepName == ApplicationConstants.WOFLOWSTEPNAME_SUBMITTED) ?? workOrderFlowSteps.First();

            workOrderCreationViewModel.SystemWorkOrderTypeFlowId = systemWorkOrderTypeFlow.SystemWorkOrderTypeFlowId;
            workOrderCreationViewModel.SystemWorkOrderFlowStepId = firstSystemWorkOrderFlowStep.SystemWorkOrderFlowStepId;
            workOrderCreationViewModel.systemWorkOrder = systemWorkOrder;


            return workOrderCreationViewModel;
        }

        private async Task<SystemCreationData> GetSystemCreationData(WorkOrderCreationViewModel creationData, CancellationToken ct = default)
        {
            SystemCreationData systemCreationData = new();

            var systemWorkOrder = await _sql.RetrieveObjectsAsync<SystemWorkOrderXrefItem>(
                new { creationData.SystemWorkOrderId },
                ct);

            systemCreationData.AppliedItems = [.. systemWorkOrder];
            systemCreationData.SystemWorkOrder = creationData.systemWorkOrder;

            return systemCreationData;
        }

        private async Task<Entity> GetEntityData(CancellationToken ct)
        {
            var entity = new Entity();

            var lookUps = await _sql.RetrieveObjectsAsync<Lookup>(
                new { LookupTypeName = LookupTypeName.EntityType.ToString() }, ct);

            var lookup = lookUps?.FirstOrDefault(a =>
                string.Equals(a.Name.Trim(), ApplicationConstants.LOOKUP_ENTITYTYPE_WORKORDER.Trim(),
                              StringComparison.OrdinalIgnoreCase));

            if (lookup != null)
            {
                entity.EntityType = lookup.LookupId;
            }

            return entity;
        }

        private async Task<Entity> AddEntityAsync(Entity entity, SqlTransaction transaction, CancellationToken ct)
        {
            Entity _newEntity = entity;
            try
            {
                _newEntity.CreationDate = DateTime.Now;
                _newEntity.CreatedBy = "dmeka";
                var dp = new DynamicParameters();
                dp.Add("EntityId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                ModelHelper.UpdateModelState(_newEntity, ObjectState.New, string.Empty, DateTime.Now);
                await _sql.SaveEntityAsync(_newEntity, dp, transaction, ct);
                var newId = dp.Get<int>("EntityId");
                _newEntity.EntityId = newId;

            }
            catch (Exception ex)
            {

            }
            return _newEntity;
        }

        public async Task<WorkOrder> AddWorkOrder(WorkOrderCreationViewModel creationData, Entity entity, SqlTransaction transaction, CancellationToken ct)
        {
            WorkOrder newWorkOrder = new()
            {
                SystemWorkOrderId = creationData.systemWorkOrder.SystemWorkOrderId,
                EstimatedClosingDate = creationData.EstimatedClosingDate,
                EffectiveDate = creationData.EffectiveDate,
                CurrentStepId = Convert.ToInt32(creationData.SystemWorkOrderFlowStepId),
                AssignedTo = creationData.AssignedToId,
                EntityId = entity.EntityId,
                SystemWorkOrderTypeFlowId = Convert.ToInt32(creationData.SystemWorkOrderTypeFlowId),
                UserId = creationData.UserId,
                OfficialNumber = creationData.OfficialNumber,
                Office = creationData.Office,
            };
            var dp = new DynamicParameters();
            dp.Add("WorkOrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            ModelHelper.UpdateModelState(newWorkOrder, ObjectState.New, "dmeka", DateTime.Now);
            await _sql.SaveEntityAsync(newWorkOrder, dp, transaction, ct);
            var newId = dp.Get<int>("WorkOrderId");
            newWorkOrder.WorkOrderId = newId;
            return newWorkOrder;
        }

        public async Task<List<WorkOrderEntity>> CreateWorkOrderEntities(WorkOrder workOrder, List<Client> lClient, SqlTransaction sqlTransaction, CancellationToken ct)
        {
            List<WorkOrderEntity> workOrderEntities = new();
            if (lClient.Count > 0)
            {
                foreach (Client client in lClient)
                {
                    WorkOrderEntity workOrderEntity = new() { WorkOrderId = workOrder.WorkOrderId, EntityId = client.EntityId };
                    workOrderEntity = await AddWorkOrderEntity(workOrderEntity, sqlTransaction, ct);
                    await AddWorkOrderClient(client, workOrderEntity, sqlTransaction, ct);
                    workOrderEntities.Add(workOrderEntity);
                }
            }
            return workOrderEntities;
        }

        public async Task<WorkOrderEntity> AddWorkOrderEntity(WorkOrderEntity workOrderEntity, SqlTransaction tran, CancellationToken ct)
        {
            WorkOrderEntity _newWOEntity = workOrderEntity;
            var dp = new DynamicParameters();
            dp.Add("WorkOrderEntityId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            ModelHelper.UpdateModelState(_newWOEntity, ObjectState.New, "dmeka", DateTime.Now);
            _newWOEntity = await _sql.SaveEntityAsync(_newWOEntity, dp, tran, ct);
            var newId = dp.Get<int>("WorkOrderEntityId");
            _newWOEntity.WorkOrderEntityId = newId;
            return _newWOEntity;
        }

        private async Task<WorkOrderClient> AddWorkOrderClient(Client client, WorkOrderEntity workOrderEntity, SqlTransaction tran, CancellationToken ct)
        {
            WorkOrderClient newWorkOrderClient = new()
            {
                ClientId = client.ClientId,
                ClientNumber = client.ClientNumber,
                WorkOrderEntityId = workOrderEntity.WorkOrderEntityId,
                ClientName = client.ClientName,
                ClientBusinessType = client.ClientBusinessType,
                Status = client.Status,
                CompanyIMONumber = client.CompanyIMONumber,
                RequirePurchaseOrder = client.RequirePurchaseOrder,
                RequirePrePayment = client.RequirePrePayment,
                WebAddress = client.WebAddress
            };

            var dp = new DynamicParameters();
            dp.Add("WorkOrderClientId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            ModelHelper.UpdateModelState(newWorkOrderClient, ObjectState.New, "dmeka", DateTime.Now);
            newWorkOrderClient = await _sql.SaveEntityAsync(newWorkOrderClient, dp, tran, ct);

            var newId = dp.Get<int>("WorkOrderClientId");
            newWorkOrderClient.WorkOrderClientId = newId;

            return newWorkOrderClient;
        }

        public async Task CreateWorkOrderItemEntities(SystemCreationData systemCreationData, WorkOrderCreationViewModel workOrderCreationViewModel, WorkOrder workOrder, List<WorkOrderEntity> lWorkOrderEntity, SqlTransaction tran, CancellationToken ct)
        {
            try
            {
                foreach (var appliedItem in systemCreationData.AppliedItems)
                {
                    WorkOrderItem newWorkOrderItem = await AddWorkOrderItem(workOrder, appliedItem, ct, tran);

                    foreach (var createdWorkOrderEntity in lWorkOrderEntity)
                    {
                        await AddWorkOrderItemEntity(newWorkOrderItem, createdWorkOrderEntity, tran, ct);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WorkOrderItem> AddWorkOrderItem(WorkOrder workOrder, SystemWorkOrderXrefItem appliedItem, CancellationToken ct, SqlTransaction transaction)
        {
            WorkOrderItem newWorkOrderItem = new WorkOrderItem()
            {
                WorkOrderId = workOrder.WorkOrderId,
                SystemWorkOrderXrefItemId = appliedItem.SystemWorkOrderXrefItemId
            };

            try
            {
                var dp = new DynamicParameters();
                dp.Add("WorkOrderItemId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                ModelHelper.UpdateModelState(newWorkOrderItem, ObjectState.New, "dmeka", DateTime.Now);
                newWorkOrderItem = await _sql.SaveEntityAsync(newWorkOrderItem, dp, transaction, ct);
                newWorkOrderItem.WorkOrderItemId = dp.Get<int>("WorkOrderItemId");
            }
            catch (Exception)
            {
                throw;
            }
            return newWorkOrderItem;
        }

        public async Task AddWorkOrderItemEntity(WorkOrderItem workOrderItem, WorkOrderEntity workOrderEntity, SqlTransaction tran, CancellationToken ct)
        {
            WorkOrderItemEntity newWorkOrderItemEntity = new()
            {
                WorkOrderEntityId = workOrderEntity.WorkOrderEntityId,
                WorkOrderItemId = workOrderItem.WorkOrderItemId
            };
            try
            {
                ModelHelper.UpdateModelState(newWorkOrderItemEntity, ObjectState.New, "dmeka", DateTime.Now);

                await _sql.SaveEntityAsync(newWorkOrderItemEntity, null, tran, ct);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddWOHistory(WorkOrderCreationViewModel creationData, WorkOrder workOrder, SqlTransaction tran, CancellationToken ct)
        {
            try
            {
                WorkOrderFlowStep newWorkOrderFlowStep = new WorkOrderFlowStep() { FlowStepId = creationData.SystemWorkOrderFlowStepId, WorkOrderId = workOrder.WorkOrderId, FlowStartDate = DateTime.Now };
                ModelHelper.UpdateModelState(newWorkOrderFlowStep, ObjectState.New, "dmeka", DateTime.Now);
                await _sql.SaveEntityAsync(newWorkOrderFlowStep, null, tran, ct);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagedResult<WorkOrderDetail>> SearchAsync(WorkOrderSearchRequest req, CancellationToken ct = default)
        {
            var page = Math.Max(1, req.Page);
            var pageSize = Math.Max(1, req.PageSize);

            var dp = new DynamicParameters();
            dp.Add("WorkOrderName", req.WorkOrderName);
            dp.Add("WorkOrderId", req.WorkOrderId.HasValue && req.WorkOrderId > 0 ? req.WorkOrderId : null);
            dp.Add("Page", req.Page);
            dp.Add("PageSize", req.PageSize);
            dp.Add("Status", req.Status);
            var workOrders = await _sql.RetrieveObjectsAsync<WorkOrderDetail>(
                dp,
                ct);

            return new PagedResult<WorkOrderDetail>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = workOrders.FirstOrDefault()?.TotalCount ?? 0,
                Items = workOrders.ToList(),
            };
        }

        public async Task<BFADetailsViewModel> GetBFAOptionsDetailsAsync(
            int workOrderId,
            CancellationToken ct = default)
        {
            var workOrder = await _sql.RetrieveObjectAsync<WorkOrder>(new { workOrderId }, ct);

            if (workOrder.WorkOrderId == 0 )
                throw new ArgumentException("Invalid workorder", nameof(workOrderId));

            string tableName = "ClientAgreement";
            var kvPairs = await _ef.ExecuteQueryAsync(ctx =>
            from wos in ctx.Set<WorkOrderSettingField>().AsNoTracking()
            join msf in ctx.Set<SystemWorkOrderSettingField>().AsNoTracking()
            on wos.SystemWorkOrderSettingFieldId equals msf.SystemWorkOrderSettingFieldId
            join sos in ctx.Set<SystemWorkOrderSetting>()
            on msf.SystemWorkOrderSettingId equals sos.SystemWorkOrderSettingId
            where wos.WorkOrderId == workOrderId
            && !wos.Removed && !msf.Removed && !sos.Removed
            && msf.TableName == tableName
            select new KeyValueDto { ColumnName = msf.ColumnName, Value = wos.Value }, ct);

            var vm = DataMappingHelper.MapKeyValuePairsTo<BFADetailsViewModel>(
                kvPairs.Select(k => (k.ColumnName ?? string.Empty, k.Value)));

            vm.ClientName = workOrder.ClientName;
            vm.AgreementText = string.IsNullOrWhiteSpace(vm.AgreementText) ? workOrder.ClientName : vm.AgreementText;
            vm.WorkOrderId = workOrderId;

            return vm;
        }

        public async Task<bool> SaveOptionsAsync(BFADetailsViewModel dto, int workOrderId,
            CancellationToken ct = default)
        {
            try
            {
                var values = new Dictionary<string, string?>
                {
                    ["IsMLCOption"] = dto.IsMLCOption ? "True" : "False",
                    ["IsISMOption"] = dto.IsISMOption ? "True" : "False",
                    ["IsISPSOption"] = dto.IsISPSOption ? "True" : "False",
                    ["HasAdditionalDiscounts"] = dto.HasAdditionalDiscounts ? "True" : "False",
                    ["EnrollmentDate"] = dto.EnrollmentDate,
                    ["AgreementText"] = dto.AgreementText
                };

                var columnNames = values.Keys.ToList();

                var mapping = await _db.Set<SystemWorkOrderSettingField>()
                    .Where(f => columnNames.Contains(f.ColumnName) && !f.Removed)
                    .GroupBy(f => f.ColumnName)
                    .Select(g => g.First())
                    .ToDictionaryAsync(f => f.ColumnName, f => f.SystemWorkOrderSettingFieldId, ct);

                var fieldIds = mapping.Values.ToList();
                var existingRows = new List<WorkOrderSettingField>();
                if (fieldIds.Count > 0)
                {
                    existingRows = await _db.Set<WorkOrderSettingField>()
                        .Where(w => w.WorkOrderId == workOrderId && fieldIds.Contains(w.SystemWorkOrderSettingFieldId))
                        .ToListAsync(ct);
                }


                var now = DateTime.UtcNow;
                var currentUser = "dmeka";

                foreach (var kv in values)
                {
                    if (!mapping.TryGetValue(kv.Key, out var fieldId))
                    {
                        continue;
                    }

                    var existing = existingRows.FirstOrDefault(e => e.SystemWorkOrderSettingFieldId == fieldId);
                    if (existing == null)
                    {
                        // insert new
                        var newRow = new WorkOrderSettingField
                        {
                            WorkOrderId = workOrderId,
                            SystemWorkOrderSettingId = 2,
                            SystemWorkOrderSettingFieldId = fieldId,
                            Value = kv.Value,
                            Removed = false,
                            CreatedBy = currentUser,
                            CreationDate = now,
                            LastUpdatedBy = currentUser,
                            LastUpdateDate = now
                        };
                        _db.Set<WorkOrderSettingField>().Add(newRow);
                    }
                    else
                    {
                        // update existing
                        existing.Value = kv.Value;
                        existing.Removed = false;
                        existing.LastUpdatedBy = currentUser;
                        existing.LastUpdateDate = now;
                        _db.Set<WorkOrderSettingField>().Update(existing);
                    }
                }
                await _db.SaveChangesAsync(ct);
                var workOrder = await _sql.RetrieveObjectAsync<WorkOrder>(new { workOrderId }, ct);
                await SaveClientAgreement(workOrderId, ct, dto);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task SaveClientAgreement(int workOrderId, CancellationToken ct, BFADetailsViewModel? bFADetailsViewModel = null)
        {
            try
            {
                var workOrder = await _sql.RetrieveObjectAsync<WorkOrder>(new { workOrderId }, ct);
                var clientAgreement =
                    await _sql.RetrieveObjectAsync<WorkOrderClientAgreement>(
                        new { workOrder.WorkOrderId }, ct);

                if (clientAgreement.WorkOrderClientAgreementId == 0)
                {
                    var itemEntity = await _sql.RetrieveObjectsAsync<WorkOrderItemEntity>
                        (new { workOrder.WorkOrderId }, ct);
                    var workOrderItemEntity = itemEntity.Where(m => m.WorkOrderItemName == ApplicationConstants.ISSUE_BFA_WORKORDER).FirstOrDefault();
                    var dp = new DynamicParameters();
                    dp.Add("SystemDiscountProgramName", ApplicationConstants.DISCOUNT_PROGRAM_BFA);
                    SystemDiscountProgram systemDiscountProgram = await _sql.RetrieveObjectAsync<SystemDiscountProgram>(new { SystemDiscountProgramName = ApplicationConstants.DISCOUNT_PROGRAM_BFA.ToString() }, ct);

                    clientAgreement = new WorkOrderClientAgreement
                    {
                        //Save the WorkOrderItemEntityId of Details item
                        WorkOrderItemEntityId = workOrderItemEntity.WorkOrderItemEntityId,
                        ClientId = workOrder.ClientId.Value,
                        SystemDiscountProgramId = systemDiscountProgram.SystemDiscountProgramId,
                        EnrollmentDate = DateTime.Now,
                        AgreementText = workOrder.ClientName
                    };


                    ModelHelper.UpdateModelState(clientAgreement, ObjectState.New, "dmeka", DateTime.Now); ;

                    await _sql.SaveEntityAsync(clientAgreement, null, null, ct);

                }
                else
                {
                    clientAgreement.IsISMOption = bFADetailsViewModel != null && bFADetailsViewModel.IsISMOption;
                    clientAgreement.IsISPSOption = bFADetailsViewModel != null && bFADetailsViewModel.IsISPSOption;
                    clientAgreement.IsMLCOption = bFADetailsViewModel != null && bFADetailsViewModel.IsMLCOption;
                    clientAgreement.HasAdditionalDiscounts = bFADetailsViewModel != null && bFADetailsViewModel.HasAdditionalDiscounts;
                    clientAgreement.AgreementText = bFADetailsViewModel != null ? bFADetailsViewModel.AgreementText : clientAgreement.AgreementText;

                    ModelHelper.UpdateModelState(clientAgreement, ObjectState.Modified, "dmeka", DateTime.Now); ;
                    await _sql.SaveEntityAsync(clientAgreement, null, null, ct);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IReadOnlyList<WorkOrder>> GetPendingWorkOrdersbyContext(int? Filter, int? systemworkorderid, bool isClientContext, CancellationToken ct = default)
        {
            IReadOnlyList<WorkOrder> workOrders = Array.Empty<WorkOrder>();

            if (isClientContext)
            {
                var results = await _sql.RetrieveObjectsAsync<WorkOrder>(
                    new { ClientId = Filter, SystemWorkOrderId = systemworkorderid }, ct);

                workOrders = results.ToList();
            }
            return workOrders;
        }
        public async Task<List<SystemWorkOrderGroup>> GetSystemWorkOrderGroup(int systemWorkOrderId, CancellationToken ct = default)
        {
            List<SystemWorkOrderGroup> systemWorkOrderGroup = new();

            var result = await _sql.RetrieveObjectsAsync<SystemWorkOrderGroup>(
                new { SystemWorkOrderId = systemWorkOrderId }, ct);
            return systemWorkOrderGroup;
        }

    }
}
