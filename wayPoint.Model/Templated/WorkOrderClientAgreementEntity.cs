using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayPoint.Model
{
    public partial class WorkOrderClientAgreementEntity : BaseModel
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
        /// Get or Set the Id property of WorkOrderClientAgreementEntity
        ///</summary>
        public int Id { get { return WorkOrderClientAgreementEntityId; } set { WorkOrderClientAgreementEntityId = value; } }

        ///<summary>
        /// Get or Set the WorkOrderClientAgreementEntityId Property of WorkOrderClientAgreementEntity
        /// WorkOrderClientAgreementEntityId is Not Nullable
        ///</summary>


        [DisplayName("Work Order Client Agreement Entity Id")]
        public int WorkOrderClientAgreementEntityId { get; set; }

        ///<summary>
        /// Get or Set the WorkOrderItemEntityId Property of WorkOrderClientAgreementEntity
        /// WorkOrderItemEntityId is Nullable 
        ///</summary>


        [DisplayName("Work Order Item Entity Id")]
        public int? WorkOrderItemEntityId { get; set; }

        ///<summary>
        /// Get or Set the WorkOrderClientAgreementId Property of WorkOrderClientAgreementEntity
        /// WorkOrderClientAgreementId is Not Nullable
        ///</summary>


        [DisplayName("Work Order Client Agreement Id")]
        public int WorkOrderClientAgreementId { get; set; }

        ///<summary>
        /// Get or Set the EntityId Property of WorkOrderClientAgreementEntity
        /// EntityId is Not Nullable
        ///</summary>


        [DisplayName("Entity Id")]
        public int EntityId { get; set; }

        ///<summary>
        /// Get or Set the WorkOrderVesselId Property of WorkOrderClientAgreementEntity
        /// WorkOrderVesselId is Nullable 
        ///</summary>


        [DisplayName("Work Order Vessel Id")]
        public int? WorkOrderVesselId { get; set; }

        ///<summary>
        /// Get or Set the SystemDiscountScheduleId Property of WorkOrderClientAgreementEntity
        /// SystemDiscountScheduleId is Nullable 
        ///</summary>


        [DisplayName("System Discount Schedule Id")]
        public int? SystemDiscountScheduleId { get; set; }

        ///<summary>
        /// Get or Set the EnrollmentDate Property of WorkOrderClientAgreementEntity
        /// EnrollmentDate is Nullable 
        ///</summary>


        [DisplayName("Enrollment Date")]
        public DateTime? EnrollmentDate { get; set; }

        ///<summary>
        /// Get or Set the AnniversaryDate Property of WorkOrderClientAgreementEntity
        /// AnniversaryDate is Nullable 
        ///</summary>


        [DisplayName("Anniversary Date")]
        public DateTime? AnniversaryDate { get; set; }

        ///<summary>
        /// Get or Set the OriginalAnniversaryYear Property of WorkOrderClientAgreementEntity
        /// OriginalAnniversaryYear is Nullable 
        ///</summary>


        [DisplayName("Original Anniversary Year")]
        public int? OriginalAnniversaryYear { get; set; }

        ///<summary>
        /// Get or Set the IsCustomFees Property of WorkOrderClientAgreementEntity
        /// IsCustomFees is Not Nullable
        ///</summary>


        [DisplayName("Is Custom Fees")]
        public bool IsCustomFees { get; set; }

        ///<summary>
        /// Get or Set the BillToClient Property of WorkOrderClientAgreementEntity
        /// BillToClient is Nullable 
        ///</summary>


        [DisplayName("Bill To Client")]
        public int? BillToClient { get; set; }

        ///<summary>
        /// Get or Set the BillingCycle Property of WorkOrderClientAgreementEntity
        /// BillingCycle is Nullable 
        ///</summary>


        [DisplayName("Billing Cycle")]
        public int? BillingCycle { get; set; }

        ///<summary>
        /// Get or Set the NoJoiningInvoice Property of WorkOrderClientAgreementEntity
        /// NoJoiningInvoice is Not Nullable
        ///</summary>


        [DisplayName("No Joining Invoice")]
        public bool NoJoiningInvoice { get; set; }

        [DisplayName("TonnageCap")]
        public float? TonnageCap { get; set; }

        [DisplayName("Price Type")]
        public int? PriceType { get; set; }

        ///<summary>
        /// Get or Set the BillingCycleCounter Property of WorkOrderClientAgreementEntity
        /// BillingCycleCounter is Nullable 
        ///</summary>


        [DisplayName("Billing Cycle Counter")]
        public int? BillingCycleCounter { get; set; }

        #endregion
    }
}
