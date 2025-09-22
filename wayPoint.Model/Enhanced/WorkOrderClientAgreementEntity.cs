using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayPoint.Model
{
    public partial class WorkOrderClientAgreementEntity
    {

        #region Properties

        public int? VesselId { get; set; }
        public int? OfficialNumber { get; set; }
        public int? IMONumber { get; set; }
        public string VesselName { get; set; }
        public bool IsSelected { get; set; }
        public int ClientId { get; set; }
        public int? VesselStatus { get; set; }
        public string VesselStatusName { get; set; }
        public string BillToClientName { get; set; }
        public string BillingCycleName { get; set; }
        public List<Lookup> BillToClientList { get; set; }
        public bool IsMLCOption { get; set; }
        public bool IsISMOption { get; set; }
        public bool IsISPSOption { get; set; }
        public string PriceTypeName { get; set; }
        public bool IsAmended { get; set; }

        #endregion
    }
}
