using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayPoint.Model    
{
    public partial class VesselClient : BaseModel
    {
        #region Properties

        ///<summary>
        /// returns the schema name of the table
        ///</summary>
        public override string SchemaName
        {
            get
            {
                return "VS.usp_";
            }
        } // end of schema name property 

        ///<summary>
        /// Get or Set the Id property of VesselClient
        ///</summary>
        public int Id { get { return VesselClientId; } set { VesselClientId = value; } }

        ///<summary>
        /// Get or Set the VesselClientId Property of VesselClient
        /// VesselClientId is Not Nullable
        ///</summary>


        [DisplayName("Vessel Client Id")]
        public int VesselClientId { get; set; }

        ///<summary>
        /// Get or Set the VesselId Property of VesselClient
        /// VesselId is Not Nullable
        ///</summary>


        [DisplayName("Vessel Id")]
        public int VesselId { get; set; }

        ///<summary>
        /// Get or Set the ClientRoleId Property of VesselClient
        /// ClientRoleId is Not Nullable
        ///</summary>


        [DisplayName("Client Role Id")]
        public int ClientRoleId { get; set; }

        ///<summary>
        /// Get or Set the FromDate Property of VesselClient
        /// FromDate is Not Nullable
        ///</summary>


        [DisplayName("From Date")]
        public DateTime FromDate { get; set; }

        ///<summary>
        /// Get or Set the ToDate Property of VesselClient
        /// ToDate is Nullable 
        ///</summary>


        [DisplayName("To Date")]
        public DateTime? ToDate { get; set; }

        ///<summary>
        /// Get or Set the LegacyId Property of VesselClient
        /// LegacyId is Nullable 
        ///</summary>


        [DisplayName("Legacy Id")]
        public int? LegacyId { get; set; }

        #endregion
    }
}
