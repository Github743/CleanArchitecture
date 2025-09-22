using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayPoint.Model.Templated
{
    public partial class VesselInspectionCertificateDetail : BaseModel
    {
        #region Properties

        ///<summary>
        /// returns the schema name of the table
        ///</summary>
        public override string SchemaName
        {
            get
            {
                return "WO.usp_";
            }
        } // end of schema name property 


        ///<summary>
        /// Get or Set the VesselId Property of VesselInspectionCertificateDetail
        /// VesselId is Nullable 
        ///</summary>


        [DisplayName("Vessel Id")]
        public int? VesselId { get; set; }

        ///<summary>
        /// Get or Set the VesselName Property of VesselInspectionCertificateDetail
        /// VesselName is Nullable 
        ///</summary>

        [StringLength(150)]
        [DisplayName("Vessel Name")]
        public string VesselName { get; set; }

        ///<summary>
        /// Get or Set the IMONumber Property of VesselInspectionCertificateDetail
        /// IMONumber is Nullable 
        ///</summary>


        [DisplayName("IMO Number")]
        public int? IMONumber { get; set; }

        ///<summary>
        /// Get or Set the OfficialNumber Property of VesselInspectionCertificateDetail
        /// OfficialNumber is Nullable 
        ///</summary>


        [DisplayName("Official Number")]
        public int? OfficialNumber { get; set; }

        ///<summary>
        /// Get or Set the WorkOrderId Property of VesselInspectionCertificateDetail
        /// WorkOrderId is Not Nullable
        ///</summary>


        [DisplayName("Work Order Id")]
        public int WorkOrderId { get; set; }

        ///<summary>
        /// Get or Set the SystemWorkOrderId Property of VesselInspectionCertificateDetail
        /// SystemWorkOrderId is Not Nullable
        ///</summary>


        [DisplayName("System Work Order Id")]
        public int SystemWorkOrderId { get; set; }

        ///<summary>
        /// Get or Set the SystemWorkOrderName Property of VesselInspectionCertificateDetail
        /// SystemWorkOrderName is Not Nullable
        ///</summary>
        [Required]
        [StringLength(150)]
        [DisplayName("System Work Order Name")]
        public string SystemWorkOrderName { get; set; }

        ///<summary>
        /// Get or Set the WorkOrderItemEntityId Property of VesselInspectionCertificateDetail
        /// WorkOrderItemEntityId is Not Nullable
        ///</summary>


        [DisplayName("Work Order Item Entity Id")]
        public int WorkOrderItemEntityId { get; set; }

        ///<summary>
        /// Get or Set the InspectionId Property of VesselInspectionCertificateDetail
        /// InspectionId is Not Nullable
        ///</summary>


        [DisplayName("Inspection Id")]
        public int InspectionId { get; set; }

        ///<summary>
        /// Get or Set the InspectionDate Property of VesselInspectionCertificateDetail
        /// InspectionDate is Nullable 
        ///</summary>


        [DisplayName("Inspection Date")]
        public DateTime? InspectionDate { get; set; }

        ///<summary>
        /// Get or Set the ClassSocietyId Property of VesselInspectionCertificateDetail
        /// ClassSocietyId is Nullable 
        ///</summary>


        [DisplayName("Class Society Id")]
        public int? ClassSocietyId { get; set; }

        ///<summary>
        /// Get or Set the ByClassSocietyId Property of VesselInspectionCertificateDetail
        /// ByClassSocietyId is Nullable 
        ///</summary>


        [DisplayName("By Class Society Id")]
        public int? ByClassSocietyId { get; set; }

        ///<summary>
        /// Get or Set the ClassSocietyName Property of VesselInspectionCertificateDetail
        /// ClassSocietyName is Nullable 
        ///</summary>

        [StringLength(150)]
        [DisplayName("Class Society Name")]
        public string ClassSocietyName { get; set; }

        ///<summary>
        /// Get or Set the IssueDate Property of VesselInspectionCertificateDetail
        /// IssueDate is Nullable 
        ///</summary>


        [DisplayName("Issue Date")]
        public DateTime? IssueDate { get; set; }

        ///<summary>
        /// Get or Set the ReIssueDate Property of VesselInspectionCertificateDetail
        /// ReIssueDate is Nullable 
        ///</summary>


        [DisplayName("Re Issue Date")]
        public DateTime? ReIssueDate { get; set; }

        ///<summary>
        /// Get or Set the ExpiryDate Property of VesselInspectionCertificateDetail
        /// ExpiryDate is Nullable 
        ///</summary>


        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        ///<summary>
        /// Get or Set the CertificateCreationDate Property of VesselInspectionCertificateDetail
        /// CertificateCreationDate is Not Nullable
        ///</summary>


        [DisplayName("Certificate Creation Date")]
        public DateTime CertificateCreationDate { get; set; }

        ///<summary>
        /// Get or Set the CertificateStatus Property of VesselInspectionCertificateDetail
        /// CertificateStatus is Nullable 
        ///</summary>

        [StringLength(150)]
        [DisplayName("Certificate Status")]
        public string CertificateStatus { get; set; }

        ///<summary>
        /// Get or Set the CertificateIssueType Property of VesselInspectionCertificateDetail
        /// CertificateIssueType is Nullable 
        ///</summary>

        [StringLength(150)]
        [DisplayName("Certificate Issue Type")]
        public string CertificateIssueType { get; set; }

        ///<summary>
        /// Get or Set the CertificateIssueTypeId Property of VesselInspectionCertificateDetail
        /// CertificateIssueTypeId is Nullable 
        ///</summary>


        [DisplayName("Certificate Issue Type Id")]
        public int? CertificateIssueTypeId { get; set; }

        ///<summary>
        /// Get or Set the CertificateId Property of VesselInspectionCertificateDetail
        /// CertificateId is Not Nullable
        ///</summary>


        [DisplayName("Certificate Id")]
        public int CertificateId { get; set; }

        ///<summary>
        /// Get or Set the CertificateStatusId Property of VesselInspectionCertificateDetail
        /// CertificateStatusId is Not Nullable
        ///</summary>


        [DisplayName("Certificate Status Id")]
        public int CertificateStatusId { get; set; }

        ///<summary>
        /// Get or Set the InspectionType Property of VesselInspectionCertificateDetail
        /// InspectionType is Nullable 
        ///</summary>

        [StringLength(150)]
        [DisplayName("Inspection Type")]
        public string InspectionType { get; set; }

        ///<summary>
        /// Get or Set the InspectionTypeLookupId Property of VesselInspectionCertificateDetail
        /// InspectionTypeLookupId is Nullable 
        ///</summary>


        [DisplayName("Inspection Type Lookup Id")]
        public int? InspectionTypeLookupId { get; set; }

        ///<summary>
        /// Get or Set the CategoryName Property of VesselInspectionCertificateDetail
        /// CategoryName is Nullable 
        ///</summary>

        [StringLength(150)]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        ///<summary>
        /// Get or Set the CategoryId Property of VesselInspectionCertificateDetail
        /// CategoryId is Nullable 
        ///</summary>


        [DisplayName("Category Id")]
        public int? CategoryId { get; set; }

        ///<summary>
        /// Get or Set the SystemDocumentName Property of VesselInspectionCertificateDetail
        /// SystemDocumentName is Nullable 
        ///</summary>

        [StringLength(150)]
        [DisplayName("System Document Name")]
        public string SystemDocumentName { get; set; }

        ///<summary>
        /// Get or Set the SystemDocumentId Property of VesselInspectionCertificateDetail
        /// SystemDocumentId is Nullable 
        ///</summary>


        [DisplayName("System Document Id")]
        public int? SystemDocumentId { get; set; }

        ///<summary>
        /// Get or Set the InspectionCertificateId Property of VesselInspectionCertificateDetail
        /// InspectionCertificateId is Nullable 
        ///</summary>


        [DisplayName("Inspection Certificate Id")]
        public int? InspectionCertificateId { get; set; }

        ///<summary>
        /// Get or Set the ReIssued Property of VesselInspectionCertificateDetail
        /// ReIssued is Nullable 
        ///</summary>


        [DisplayName("Re Issued")]
        public bool? ReIssued { get; set; }

        ///<summary>
        /// Get or Set the IntermediateInspectionDate Property of VesselInspectionCertificateDetail
        /// IntermediateInspectionDate is Nullable 
        ///</summary>


        [DisplayName("Intermediate Inspection Date")]
        public DateTime? IntermediateInspectionDate { get; set; }

        ///<summary>
        /// Get or Set the IntermediatePortCode Property of VesselInspectionCertificateDetail
        /// IntermediatePortCode is Nullable 
        ///</summary>

        [StringLength(202)]
        [DisplayName("Intermediate Port Code")]
        public string IntermediatePortCode { get; set; }

        ///<summary>
        /// Get or Set the IntermediateVerification Property of VesselInspectionCertificateDetail
        /// IntermediateVerification is Nullable 
        ///</summary>


        [DisplayName("Intermediate Verification")]
        public bool IntermediateVerification { get; set; }

        ///<summary>
        /// Get or Set the WorkOrderDocumentId Property of VesselInspectionCertificateDetail
        /// WorkOrderDocumentId is Nullable 
        ///</summary>


        [DisplayName("Work Order Document Id")]
        public int? WorkOrderDocumentId { get; set; }

        ///<summary>
        /// Get or Set the WorkOrderDocumentStreamId Property of VesselInspectionCertificateDetail
        /// WorkOrderDocumentStreamId is Nullable 
        ///</summary>


        //[DisplayName("Work Order Document Stream Id")]
        //public Guid? WorkOrderDocumentStreamId { get; set; }

        [DisplayName("Work Order Document File Name")]
        public string WorkOrderDocumentFileName { get; set; }

        #endregion
    }
}
