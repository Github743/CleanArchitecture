using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WayPoint.Model.Templated;

namespace WayPoint.Model
{
    public partial class VesselClient
    {
        public WorkOrderVesselClient GetWorkOrderVesselClient()
        {
            WorkOrderVesselClient workOrderVesselClient = new WorkOrderVesselClient()
            {
                VesselClientId = VesselClientId,
                ClientRoleId = ClientRoleId,
            };

            return workOrderVesselClient;
        }

        public int? IMONumber { get; set; }
        public int? OfficialNumber { get; set; }
        public string Name { get; set; }
        public int? EntityId { get; set; }
        public string VesselTypeName { get; set; }
        public string ClientName { get; set; }
        public int? ClientId { get; set; }
        public string ClientTypeName { get; set; }
        public string ClientType { get; set; }
        public string DisplayName { get; set; }
        public string VesselStatus { get; set; }
        public int ClientRole { get; set; }
        public int? FlagState { get; set; }
        public string VesselLookup { get; set; }
        public int? ClientNumber { get; set; }
        public float? NetTon { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string FlagStateName { get; set; }
        public bool HasAlerts { get; set; }
        public int? ClientEntityId { get; set; }

        public DateTime? DeletionDate { get; set; }
        public string ParentVesselType { get; set; }
        public int? ClassSocietyId { get; set; }
        public string ClassSocietyName { get; set; }
        public string PlaceBuilt { get; set; }
        public string Builder { get; set; }
        public int? BuiltYear { get; set; }
        public double? LOA { get; set; }
        public int? Age { get; set; }
        public string BareBoatType { get; set; }
        public string CompanyIMONumber { get; set; }

        public string Country { get; set; }

        public string LegacyValue { get; set; }

        public bool IsVesselSanctioned { get; set; }
    }
}
