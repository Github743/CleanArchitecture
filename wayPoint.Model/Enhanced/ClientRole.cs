using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayPoint.Model
{
    public partial class ClientRole
    {
        public string BusinessDepartmentName { get; set; }
        public string BusinessDivisionName { get; set; }
        public string ClientRoleName { get; set; }
        public string LegacyValueClientRole { get; set; }
        public string ClientRoleTypeName { get; set; }
        public int ClientRoleLookupId { get; set; }
        public string ClientName { get; set; }
        public string ClientNumber { get; set; }
        public string ClientLookup { get; set; }
        public string ClientStatus { get; set; }
        public int ClientEntityId { get; set; }
        public string CompanyIMONumber { get; set; }
        public string Country { get; set; }

    }//end class
}
