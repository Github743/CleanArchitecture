using WayPoint.Model;
using WayPoint.Model.Templated;
using WayPoint_Infrastructure.Data;
using WayPoint_Infrastructure.Interfaces;

namespace WayPoint_Infrastructure.Repositories
{
    public class SystemDiscountRepository : ISystemDiscountRepository
    {
        private readonly ISqlEngine _sql;
        private readonly ILookUpRepository _lookupRepository;

        public SystemDiscountRepository(ISqlEngine sql,ILookUpRepository lookUpRepository)
        {
            _sql = sql ?? throw new ArgumentNullException(nameof(sql));
            _lookupRepository = lookUpRepository;
        }

        public async Task<IReadOnlyList<SystemDiscountSchedules>> GetDiscountSchedules(
            int workOrderId,
            int systemDiscountProgramId,
            CancellationToken ct = default)
        {
            // null/empty default result
            var empty = Array.Empty<SystemDiscountSchedules>();
            systemDiscountProgramId = 3;
            // fetch agreement (pass anonymous object for parameter)
            var workOrderClientAgreement = await _sql.RetrieveObjectAsync<WorkOrderClientAgreement>(
                new { WorkOrderId = workOrderId, SystemDiscountProgramId = systemDiscountProgramId },
                ct);

            // If none found, return empty list
            if (workOrderClientAgreement == null)
                return empty;

            // fetch discount schedules using values from agreement
            var discountSchedules = await _sql.RetrieveObjectsAsync<SystemDiscountSchedules>(
                new
                {
                    SystemDiscountProgramId = systemDiscountProgramId,
                    MLC = workOrderClientAgreement.IsMLCOption,
                    ISM = workOrderClientAgreement.IsISMOption,
                    ISPS = workOrderClientAgreement.IsISPSOption
                },
                ct);

            // if the engine returns null (defensive), return empty
            return discountSchedules ?? empty;
        }

        public async Task<IReadOnlyList<SystemDiscountScheduleProducts>> GetDiscountScheduleProducts(
           int systemDiscountScheduleId,
           CancellationToken ct = default)
        {
            // null/empty default result
            var empty = Array.Empty<SystemDiscountScheduleProducts>();

            // fetch agreement (pass anonymous object for parameter)
            var discountProducts = await _sql.RetrieveObjectsAsync<SystemDiscountScheduleProducts>(
                new { SystemDiscountScheduleId = systemDiscountScheduleId },
                ct);

            // If none found, return empty list
            if (discountProducts == null)
                return empty;

            return discountProducts.OrderBy(x => x.DefaultOrder).ToList();
        }

        public async Task<IReadOnlyList<SystemProductDiscountGroup>> GetSystemProductDiscountGroupByName(
          string systemProductName,
          CancellationToken ct = default)
        {
            var empty = Array.Empty<SystemProductDiscountGroup>();

            var discountProductsdiscount = await _sql.RetrieveObjectsAsync<SystemProductDiscountGroup>(
                new { SystemProductNameSearchText = systemProductName },
                ct);

            if (discountProductsdiscount == null)
                return empty;

            return discountProductsdiscount ?? empty;
        }
        public async Task<IReadOnlyList<WorkOrderClientAgreementEntityProduct>> GeAdditionalDiscountWOClientAgreementProducts(
         int workOrderClientAgreementId, int? systemProductId,
         CancellationToken ct = default)
        {
            IReadOnlyList<WorkOrderClientAgreementEntityProduct> ldiscountProducts = Array.Empty<WorkOrderClientAgreementEntityProduct>();
            if (systemProductId != null && systemProductId > 0)
            {
                var discountProductsLists = await _sql.RetrieveObjectsAsync<WorkOrderClientAgreementEntityProduct>(
                    new { WorkOrderClientAgreementId = workOrderClientAgreementId, SystemProductId = systemProductId },
                    ct);
                ldiscountProducts = discountProductsLists.Where(x=>x.IsAdditionalDiscount==true).OrderBy(e=>e.Amount).ToList();
            }
            else
            {
                var discountProductsList = await _sql.RetrieveObjectsAsync<WorkOrderClientAgreementEntityProduct>(
                                    new { WorkOrderClientAgreementId = workOrderClientAgreementId},
                                    ct);
                ldiscountProducts = discountProductsList.Where(x => x.IsAdditionalDiscount == true && x.WorkOrderClientAgreementEntityId == null).OrderBy(e => e.Amount).ToList();
            }

            return ldiscountProducts ;
        }
        public async Task<IReadOnlyList<WorkOrderClientAgreementEntity>> GetWorkOrderClientAgreementEntities(
         int workOrderClientAgreementId, int clientId,
         CancellationToken ct = default)
        {
            IReadOnlyList<WorkOrderClientAgreementEntity> lworkOrderClientAgreement = Array.Empty<WorkOrderClientAgreementEntity>();
            WorkOrderClientAgreement _workOrderClientAgreement = new();
            WorkOrder wo = new();
            var existingWOClientAgreementEntities = await _sql.RetrieveObjectsAsync<WorkOrderClientAgreementEntity>(
                new { WorkOrderClientAgreementId = workOrderClientAgreementId},ct);
            lworkOrderClientAgreement = existingWOClientAgreementEntities.Where(x => x.WorkOrderVesselId !=null).ToList();


             _workOrderClientAgreement = await _sql.RetrieveObjectAsync<WorkOrderClientAgreement>(
               new { WorkOrderClientAgreementId = workOrderClientAgreementId }, ct);

            wo = await _sql.RetrieveObjectAsync<WorkOrder>(
               new { WorkOrderId = _workOrderClientAgreement.WorkOrderId }, ct);
            bool isBFAWorkOrder = (wo.DisplayName == ApplicationConstants.AMEND_BFA_WORKORDER || wo.DisplayName == ApplicationConstants.ISSUE_BFA_WORKORDER) ? true : false;
            if (wo.WorkOrderStatus == ApplicationConstants.ACTIVE)
            {
                foreach (var entity in existingWOClientAgreementEntities)
                {
                    entity.IsSelected = true;
                }
                lworkOrderClientAgreement = existingWOClientAgreementEntities.ToList();
            }
            else
            {
                List<VesselClient> vesselClients = new();
                List<int> clientIdList = new List<int>();
                clientIdList.Add(clientId);
                List<ClientRole> existingClientRoles = new List<ClientRole>();
                // retrive list of client role
                existingClientRoles = (List<ClientRole>)await _sql.RetrieveObjectsAsync<ClientRole>(
               new { ClientId = clientId }, ct);
                if (existingClientRoles != null)
                {
                    //adding current client
                    //check if the client has Owning Group role
                    if (existingClientRoles.Any(x => x.ClientRoleName.ToLower().Equals(ApplicationConstants.CLIENT_ROLE_OWNING_GROUP, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        var ogClients = await GetClientsOfOwningGroup(clientId);
                        if (ogClients != null && ogClients.Count > 0)
                        {
                            if (wo.DisplayName == ApplicationConstants.ISSUE_BFA_WORKORDER)
                                ogClients = ogClients.Where(x => !x.AffiliatedClientRoles.Contains(ApplicationConstants.CLIENT_ROLE_BFA)).ToList();

                            clientIdList.AddRange(ogClients.Distinct().Select(x => x.AffiliatedClientId));
                        }
                    }
                }
                vesselClients = await GetVesselsByClientIdList(clientIdList, ct);
                if (vesselClients != null && vesselClients.Count > 0)
                {
                    List<int> agreementVesselIdsList = existingWOClientAgreementEntities.Select(x => x.VesselId.Value).ToList();
                    List<int> vesselIdList = vesselClients.Where(x => (x.VesselStatus != ApplicationConstants.LOOKUPTYPE_VESSEL_STATUS_STRICKEN) && (x.ToDate == null)).Select(x => x.VesselId).ToList();
                    string bfaorADAVesselIdsList = string.Join(",", vesselIdList);
                    List<VesselClient> vesselClientsByVesselId =await GetVesselClientsByVesselIdList(bfaorADAVesselIdsList);
                    if (vesselClientsByVesselId != null && vesselClientsByVesselId.Count > 0)
                    {
                        if (isBFAWorkOrder)
                        {
                            //get vessels which are associated with any other bfa enrolled client
                            vesselClientsByVesselId = vesselClientsByVesselId.Where(x => x.ClientTypeName == ApplicationConstants.CLIENT_ROLE_BFA && !clientIdList.Contains(x.ClientId.Value)).ToList();
                            vesselIdList = vesselIdList.Except(vesselClientsByVesselId.Select(x => x.VesselId)).ToList();
                        }
                    }
                    agreementVesselIdsList.AddRange(vesselIdList);
                    List<int> filteredVesselIdList = agreementVesselIdsList.Distinct().ToList();
                    string vesselIdsList = string.Join(",", filteredVesselIdList);
                    List<VesselClient> filterdVesselClients = await GetFilteredVesselClients(vesselClients, filteredVesselIdList, ct);

                    //get only ISM Clients
                    var defaultBillToClients = await GetVesselClientsByVesselIdList(vesselIdsList, ct);
                    defaultBillToClients=defaultBillToClients.Where(x=>x.ClientTypeName==ApplicationConstants.CLIENT_ROLE_ISM)
                                        .GroupBy(i=> new {i.VesselId})
                                        .Select(e=>new VesselClient()
                                        { 
                                            VesselId = e.First().VesselId,
                                            ClientId = e.First().ClientId,
                                            ClientName = e.First().ClientName,
                                            FlagStateName = e.First().FlagStateName,
                                            Name = e.First().Name,
                                            VesselStatus = e.First().VesselStatus,
                                            ClientTypeName = e.First().ClientTypeName
                                        }).ToList();
                    var anniversaryDates =await GetAnniversaryDatesPerVessel(workOrderClientAgreementId, vesselIdsList,ct);
                    var lBillingCycleLookup = await _lookupRepository.GetLookupsByTypeName(ApplicationConstants.BILLING_CYCLE_YEAR,ct);
                    Lookup? defaultLookup = lBillingCycleLookup.Where(x => x.LegacyValue == "0").FirstOrDefault();
                    DateTime _enrollmentDate = Convert.ToDateTime(_workOrderClientAgreement.EnrollmentDate);
                    lworkOrderClientAgreement = filterdVesselClients.LeftJoin(existingWOClientAgreementEntities,
                                                                  l => l.EntityId,
                                                                  r => r.EntityId,
                                                                  (l, r) => new WorkOrderClientAgreementEntity
                                                                  {
                                                                      ClientId = r != null ? r.ClientId : l.ClientId.Value,
                                                                      VesselId = r != null ? r.VesselId : l.VesselId,
                                                                      VesselName = r != null ? r.VesselName : l.Name,
                                                                      IMONumber = r != null ? r.IMONumber : l.IMONumber,
                                                                      OfficialNumber = r != null ? r.OfficialNumber : l.OfficialNumber,
                                                                      //VesselStatus = r != null ? r.VesselStatus : l.VesselStatus,
                                                                      VesselStatusName = l != null ? l.VesselStatus : r.VesselStatusName,
                                                                      EntityId = r != null ? r.EntityId : l.EntityId.GetValueOrDefault(),

                                                                      WorkOrderVesselId = r != null ? r.WorkOrderVesselId : -1,
                                                                      WorkOrderClientAgreementEntityId = r != null ? r.WorkOrderClientAgreementEntityId : -1,
                                                                      WorkOrderItemEntityId = r != null ? r.WorkOrderItemEntityId : null,
                                                                      WorkOrderClientAgreementId = r != null ? r.WorkOrderClientAgreementId : workOrderClientAgreementId,
                                                                      SystemDiscountScheduleId = r != null ? r.SystemDiscountScheduleId : null,
                                                                      //EnrollmentDate = r != null ? r.EnrollmentDate : workOrderClientAgreement.EnrollmentDate,
                                                                      EnrollmentDate = r != null ? r.EnrollmentDate : _enrollmentDate,
                                                                      AnniversaryDate = (r != null && r.AnniversaryDate.HasValue) ? r.AnniversaryDate : (anniversaryDates.ContainsKey(l.VesselId) ? anniversaryDates[l.VesselId] : null),
                                                                      //OriginalAnniversaryYear = r != null ? r.OriginalAnniversaryYear : ((anniversaryDates.ContainsKey(l.VesselId) && anniversaryDates[l.VesselId].HasValue) ? (int?)Convert.ToDateTime(anniversaryDates[l.VesselId]).Year : null),
                                                                      OriginalAnniversaryYear = isBFAWorkOrder ? (r != null ? r.OriginalAnniversaryYear : null) : null, //Saving OriginalAnniversaryYear in the Vessel's tab Save functionality

                                                                      IsCustomFees = r != null ? r.IsCustomFees : false,
                                                                      BillToClient = isBFAWorkOrder ? (r != null ? r.BillToClient : (defaultBillToClients.Any(x => x.VesselId == l.VesselId) ? defaultBillToClients.FirstOrDefault(x => x.VesselId == l.VesselId).ClientId : null)) : null,
                                                                      BillToClientName = isBFAWorkOrder ? (r != null ? r.BillToClientName : (defaultBillToClients.Any(x => x.VesselId == l.VesselId) ? defaultBillToClients.FirstOrDefault(x => x.VesselId == l.VesselId).ClientName : "")) : null,

                                                                      BillingCycle = isBFAWorkOrder ? (r != null ? r.BillingCycle : (anniversaryDates.ContainsKey(l.VesselId) ? (int?)GetBillingCycleLookup(anniversaryDates[l.VesselId], lBillingCycleLookup, ct).LookupId : defaultLookup.LookupId)) : null,
                                                                      BillingCycleName = isBFAWorkOrder ? (r != null ? r.BillingCycleName : (anniversaryDates.ContainsKey(l.VesselId) ? GetBillingCycleLookup(anniversaryDates[l.VesselId], lBillingCycleLookup, ct).Name : defaultLookup.Name)) : null,
                                                                      NoJoiningInvoice = r != null ? r.NoJoiningInvoice : false,
                                                                      IsSelected = (r != null && r.WorkOrderClientAgreementEntityId > 0) ? true : false,
                                                                  }).OrderByDescending(x => x.IsSelected).ToList();
                }
            }
                return lworkOrderClientAgreement;
        }

        public async Task<IReadOnlyList<ClientAffiliationDetail>> GetClientsOfOwningGroup(int clientId, CancellationToken ct = default)
        {
            IReadOnlyList<ClientAffiliationDetail> result = Array.Empty<ClientAffiliationDetail>();
            result = await _sql.RetrieveObjectsAsync<ClientAffiliationDetail>(
              new { ClientId = clientId }, ct);
            return result;
        }

        public async Task<List<VesselClient>> GetVesselsByClientIdList(List<int> clientIdList, CancellationToken ct = default)
        {
            List<VesselClient> filteredVesselClients = new List<VesselClient>();
            string clientids = string.Join(",", clientIdList);
            var vesselClients = await _sql.RetrieveObjectsAsync<VesselClient>(
                 new { ClientIdList = clientids }, ct);
            filteredVesselClients = vesselClients.Where(x => (!string.IsNullOrEmpty(x.VesselStatus))
                                                               && (x.FlagStateName.ToLower() == ApplicationConstants.FLAGSTATE_LIBERIA.ToLower()
                                                                           && (x.VesselStatus.ToLower() == ApplicationConstants.LOOKUPTYPE_VESSEL_STATUS_ACTIVE.ToLower()
                                                                           || x.VesselStatus.ToLower() == ApplicationConstants.LOOKUPTYPE_VESSEL_STATUS_PENDING.ToLower()
                                                                           || x.VesselStatus.ToLower() == ApplicationConstants.LOOKUPTYPE_VESSEL_STATUS_LAIDUP.ToLower()
                                                                           || x.VesselStatus.ToLower() == ApplicationConstants.LOOKUPTYPE_VESSEL_STATUS_STRICKEN.ToLower())))
                             .GroupBy(item => new { item.VesselId, item.OfficialNumber, item.IMONumber })
                             .Select(y => new VesselClient()
                             {
                                 VesselId = y.First().VesselId,
                                 OfficialNumber = y.First().OfficialNumber,
                                 IMONumber = y.First().IMONumber,
                                 ClientId = y.First().ClientId,
                                 ClientName = y.First().ClientName,
                                 FlagState = y.First().FlagState,
                                 FlagStateName = y.First().FlagStateName,
                                 Name = y.First().Name,
                                 RegistrationDate = y.First().RegistrationDate,
                                 VesselStatus = y.First().VesselStatus,
                                 EntityId = y.First().EntityId,
                                 DisplayName = string.Join(", ", y.Distinct().Select(e => e.ClientTypeName).ToArray()),
                                 VesselTypeName = y.FirstOrDefault().VesselTypeName,
                                 ToDate = y.FirstOrDefault().ToDate

                             })
                             .ToList();
            return filteredVesselClients;
        }
        public async Task<List<VesselClient>> GetVesselClientsByVesselIdList(string VesselIdsList, CancellationToken ct = default)
        {
            List<VesselClient> vesselClients = new List<VesselClient>();
           var lvesselClients = await _sql.RetrieveObjectsAsync<VesselClient>(
                new { VesselIdList = VesselIdsList }, ct);
            vesselClients = lvesselClients.ToList();
            return vesselClients;
        }
        public async Task<Dictionary<int, DateTime>> GetAnniversaryDatesPerVessel(int workOrderClientAgreementId, string vesselIdsList, CancellationToken ct = default)
        {
            WorkOrderClientAgreement woClientAgreement = await _sql.RetrieveObjectAsync<WorkOrderClientAgreement>(
                new { WorkOrderClientAgreementId = workOrderClientAgreementId }, ct);
           
            string categroyLookupIds =string.Empty;
            if (woClientAgreement != null)
            {
               var insTypeLookup =await _lookupRepository.GetLookupsByTypeName(ApplicationConstants.LOOKUPTYPE_INSPECTION_TYPE_CATEGORY,ct);
               var lookupMap = insTypeLookup.ToDictionary(x => x.Name, x => x.LookupId);
                List<int> ids = new List<int>();
                if (woClientAgreement.IsISMOption && lookupMap.TryGetValue(ApplicationConstants.INSPECTION_CATEGORY_ISM, out var ismId))
                    ids.Add(ismId);

                if (woClientAgreement.IsISPSOption && lookupMap.TryGetValue(ApplicationConstants.INSPECTION_CATEGORY_ISPS, out var ispsId))
                    ids.Add(ispsId);

                if (woClientAgreement.IsMLCOption && lookupMap.TryGetValue(ApplicationConstants.INSPECTION_CATEGORY_MLC, out var mlcId))
                    ids.Add(mlcId);
                categroyLookupIds = string.Join(",", ids);
            }
            var lvesselinspeccertDeatils = await _sql.RetrieveObjectsAsync<VesselInspectionCertificateDetail>(
                new { VesselIdList = vesselIdsList, CategoryIdList= categroyLookupIds }, ct);
            return lvesselinspeccertDeatils
         .Where(x =>
             (x.InspectionType == ApplicationConstants.INSPECTION_TYPE_INTIAL ||
              x.InspectionType == ApplicationConstants.INSPECTION_TYPE_RENEWAL ||
              x.InspectionType == ApplicationConstants.INSPECTION_TYPE_INTERMEDIATE) &&
             x.CertificateStatus == ApplicationConstants.CERTIFICATE_STATUS_ACTIVE)
         .GroupBy(x => x.VesselId)
         .Select(g => g.OrderByDescending(x => x.InspectionDate).First())
         .ToDictionary(x => x.VesselId!.Value, x => x.InspectionDate!.Value);
        }

        public async Task<List<VesselClient>> GetFilteredVesselClients(List<VesselClient> vesselClients, List<int> filteredVesselIdList,CancellationToken ct = default)
        {
            //filter vesselclients records with final list of vessel ids
            List<VesselClient> filterdVesselClients = vesselClients.Where(x => (filteredVesselIdList.Contains(x.VesselId)))
                                                                           .GroupBy(item => new { item.VesselId, item.OfficialNumber, item.IMONumber })
                                                                           .Select(y => new VesselClient()
                                                                           {
                                                                               VesselId = y.First().VesselId,
                                                                               OfficialNumber = y.First().OfficialNumber,
                                                                               IMONumber = y.First().IMONumber,
                                                                               ClientId = y.First().ClientId,
                                                                               ClientName = y.First().ClientName,
                                                                               FlagState = y.First().FlagState,
                                                                               FlagStateName = y.First().FlagStateName,
                                                                               Name = y.First().Name,
                                                                               RegistrationDate = y.First().RegistrationDate,
                                                                               VesselStatus = y.First().VesselStatus,
                                                                               EntityId = y.First().EntityId,
                                                                               DisplayName = string.Join(", ", y.Distinct().Select(e => e.ClientTypeName).ToArray()),
                                                                               VesselTypeName = y.FirstOrDefault().VesselTypeName,
                                                                               ToDate = y.FirstOrDefault().ToDate

                                                                           }).ToList();
            return filterdVesselClients;
        }
        public Lookup GetBillingCycleLookup(DateTime? anniversaryDate, IReadOnlyList<Lookup> billingCycleLookup,CancellationToken ct=default)
        {
            //calculate billing cycle based on anniversary date
            if (anniversaryDate.HasValue)
            {
                int yearDifference = (DateTime.Now.Year - anniversaryDate.Value.Year);

                //for anniversary date and current date year difference more than 9 billing cycle will start from inital subsequent on wards 
                if (yearDifference > 9)
                    yearDifference = (yearDifference % 9) + 4;
                //adding plus 4 to reminder as we have to skip Initial(legacyvalue-0),1st annual(legacyvalue-1),2nd annual(legacyvalue-2), 3rd annual(legacyvalue-3) and 4th annual(legacyvalue-4)

                return billingCycleLookup.Where(x => x.LegacyValue == yearDifference.ToString()).FirstOrDefault();
            }
            //if no anniversary date then return Initial(legacyvalue-0) as billing cycle
            return billingCycleLookup.Where(x => x.LegacyValue == "0").FirstOrDefault();
        }

    }
}
