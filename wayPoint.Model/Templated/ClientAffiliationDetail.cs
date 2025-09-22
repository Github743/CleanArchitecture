using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayPoint.Model.Templated
{
    public partial class ClientAffiliationDetail : BaseModel
    {
        #region Properties

        ///<summary>
        /// returns the schema name of the table
        ///</summary>
        public override string SchemaName
        {
            get
            {
                return "FN";
            }
        } // end of schema name property 


        ///<summary>
        /// Get or Set the ClientAffiliationId Property of ClientAffiliationDetail
        /// ClientAffiliationId is Not Nullable
        ///</summary>


        [DisplayName("Client Affiliation Id")]
        public int ClientAffiliationId { get; set; }

        ///<summary>
        /// Get or Set the ClientId Property of ClientAffiliationDetail
        /// ClientId is Not Nullable
        ///</summary>


        [DisplayName("Client Id")]
        public int ClientId { get; set; }

        ///<summary>
        /// Get or Set the ClientName Property of ClientAffiliationDetail
        /// ClientName is Not Nullable
        ///</summary>
        [Required]
        [StringLength(250)]
        [DisplayName("Client Name")]
        public string ClientName { get; set; }

        ///<summary>
        /// Get or Set the ClientNumber Property of ClientAffiliationDetail
        /// ClientNumber is Not Nullable
        ///</summary>


        [DisplayName("Client Number")]
        public int ClientNumber { get; set; }

        ///<summary>
        /// Get or Set the AffiliatedClientId Property of ClientAffiliationDetail
        /// AffiliatedClientId is Not Nullable
        ///</summary>


        [DisplayName("Affiliated Client Id")]
        public int AffiliatedClientId { get; set; }

        ///<summary>
        /// Get or Set the FromDate Property of ClientAffiliationDetail
        /// FromDate is Not Nullable
        ///</summary>


        [DisplayName("From Date")]
        public DateTime FromDate { get; set; }

        ///<summary>
        /// Get or Set the ToDate Property of ClientAffiliationDetail
        /// ToDate is Nullable 
        ///</summary>


        [DisplayName("To Date")]
        public DateTime? ToDate { get; set; }

        ///<summary>
        /// Get or Set the InheritDiscounts Property of ClientAffiliationDetail
        /// InheritDiscounts is Not Nullable
        ///</summary>


        [DisplayName("Inherit Discounts")]
        public bool InheritDiscounts { get; set; }

        ///<summary>
        /// Get or Set the AffiliatedClientName Property of ClientAffiliationDetail
        /// AffiliatedClientName is Not Nullable
        ///</summary>
        [Required]
        [StringLength(250)]
        [DisplayName("Affiliated Client Name")]
        public string AffiliatedClientName { get; set; }

        ///<summary>
        /// Get or Set the AffiliatedClientNumber Property of ClientAffiliationDetail
        /// AffiliatedClientNumber is Not Nullable
        ///</summary>


        [DisplayName("Affiliated Client Number")]
        public int AffiliatedClientNumber { get; set; }

        ///<summary>
        /// Get or Set the AffiliatedClientEntityId Property of ClientAffiliationDetail
        /// AffiliatedClientEntityId is Not Nullable
        ///</summary>

        [DisplayName("Affiliated Client EntityId")]
        public int AffiliatedClientEntityId { get; set; }

        ///<summary>
        /// Get or Set the AffiliatedClientRoles Property of ClientAffiliationDetail
        /// AffiliatedClientRoles is Nullable 
        ///</summary>

        [StringLength(4096)]
        [DisplayName("Affiliated Client Roles")]
        public string AffiliatedClientRoles { get; set; }

        ///<summary>
        /// Get or Set the AffiliatedClientStatus Property of ClientAffiliationDetail
        /// AffiliatedClientStatus is Nullable 
        ///</summary>

        [StringLength(150)]
        [DisplayName("Affiliated Client Status")]
        public string AffiliatedClientStatus { get; set; }

        ///<summary>
        /// Get or Set the Address Property of ClientAffiliationDetail
        /// Address is Not Nullable
        ///</summary>
        [Required]
        [StringLength(752)]
        [DisplayName("Address")]
        public string Address { get; set; }

        ///<summary>
        /// Get or Set the City Property of ClientAffiliationDetail
        /// City is Nullable 
        ///</summary>

        [StringLength(50)]
        [DisplayName("City")]
        public string City { get; set; }

        ///<summary>
        /// Get or Set the County Property of ClientAffiliationDetail
        /// County is Nullable 
        ///</summary>

        [StringLength(50)]
        [DisplayName("County")]
        public string County { get; set; }

        ///<summary>
        /// Get or Set the State Property of ClientAffiliationDetail
        /// State is Nullable 
        ///</summary>

        [StringLength(50)]
        [DisplayName("State")]
        public string State { get; set; }

        ///<summary>
        /// Get or Set the Province Property of ClientAffiliationDetail
        /// Province is Nullable 
        ///</summary>

        [StringLength(50)]
        [DisplayName("Province")]
        public string Province { get; set; }

        ///<summary>
        /// Get or Set the PostalCode Property of ClientAffiliationDetail
        /// PostalCode is Nullable 
        ///</summary>

        [StringLength(50)]
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        ///<summary>
        /// Get or Set the Country Property of ClientAffiliationDetail
        /// Country is Nullable 
        ///</summary>

        [StringLength(150)]
        [DisplayName("Country")]
        public string Country { get; set; }

        ///<summary>
        /// Get or Set the ActiveVesselCount Property of ClientAffiliationDetail
        /// ActiveVesselCount is Nullable 
        ///</summary>


        [DisplayName("Active Vessel Count")]
        public int? ActiveVesselCount { get; set; }

        ///<summary>
        /// Get or Set the LaidupVesselCount Property of ClientAffiliationDetail
        /// LaidupVesselCount is Nullable 
        ///</summary>


        [DisplayName("Laidup Vessel Count")]
        public int? LaidupVesselCount { get; set; }

        ///<summary>
        /// Get or Set the PendingVesselCount Property of ClientAffiliationDetail
        /// PendingVesselCount is Nullable 
        ///</summary>


        [DisplayName("Pending Vessel Count")]
        public int? PendingVesselCount { get; set; }

        ///<summary>
        /// Get or Set the ActiveBareboatOutVesselCount Property of ClientAffiliationDetail
        /// ActiveBareboatOutVesselCount is Nullable 
        ///</summary>


        [DisplayName("Active Bareboat Out Vessel Count")]
        public int? ActiveBareboatOutVesselCount { get; set; }



        ///<summary>
        /// Get or Set the OwningGroupClientCountry Property of ClientAffiliationDetail
        /// OwningGroupClientCountry is Nullable 
        ///</summary>

        [StringLength(150)]
        [DisplayName("Owning Group Client Country")]
        public string OwningGroupClientCountry { get; set; }
        #endregion
    }
}
