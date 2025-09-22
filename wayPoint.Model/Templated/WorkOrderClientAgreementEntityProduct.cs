using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayPoint.Model
{
   public partial class WorkOrderClientAgreementEntityProduct : BaseModel
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
        /// Get or Set the Id property of WorkOrderClientAgreementEntityProduct
        ///</summary>
        public  int Id { get { return WorkOrderClientAgreementEntityProductId; } set { WorkOrderClientAgreementEntityProductId = value; } }

        ///<summary>
        /// Get or Set the WorkOrderClientAgreementEntityProductId Property of WorkOrderClientAgreementEntityProduct
        /// WorkOrderClientAgreementEntityProductId is Not Nullable
        ///</summary>


        [DisplayName("Work Order Client Agreement Entity Product Id")]
        public int WorkOrderClientAgreementEntityProductId { get; set; }

        ///<summary>
        /// Get or Set the WorkOrderClientAgreementEntityId Property of WorkOrderClientAgreementEntityProduct
        /// WorkOrderClientAgreementEntityId is Nullable 
        ///</summary>


        [DisplayName("Work Order Client Agreement Entity Id")]
        public int? WorkOrderClientAgreementEntityId { get; set; }

        ///<summary>
        /// Get or Set the WorkOrderClientAgreementId Property of WorkOrderClientAgreementEntityProduct
        /// WorkOrderClientAgreementId is Not Nullable
        ///</summary>


        [DisplayName("Work Order Client Agreement Id")]
        public int WorkOrderClientAgreementId { get; set; }

        ///<summary>
        /// Get or Set the SystemProductId Property of WorkOrderClientAgreementEntityProduct
        /// SystemProductId is Not Nullable
        ///</summary>


        [DisplayName("System Product Id")]
        public int SystemProductId { get; set; }

        ///<summary>
        /// Get or Set the DiscountType Property of WorkOrderClientAgreementEntityProduct
        /// DiscountType is Nullable 
        ///</summary>


        [DisplayName("Discount Type")]
        public int? DiscountType { get; set; }

        ///<summary>
        /// Get or Set the Amount Property of WorkOrderClientAgreementEntityProduct
        /// Amount is Nullable 
        ///</summary>


        [DisplayName("Amount")]
        public decimal? Amount { get; set; }

        ///<summary>
        /// Get or Set the IsCustomized Property of WorkOrderClientAgreementEntityProduct
        /// IsCustomized is Not Nullable
        ///</summary>


        [DisplayName("Is Customized")]
        public bool IsCustomized { get; set; }

        ///<summary>
        /// Get or Set the IsAddtionalDiscount Property of WorkOrderClientAgreementEntityProduct
        /// IsAddtionalDiscount is Not Nullable
        ///</summary>


        [DisplayName("Is Additional Discount")]
        public bool IsAdditionalDiscount { get; set; }

        ///<summary>
        /// Get or Set the LimitPerYear Property of WorkOrderClientAgreementEntityProduct
        /// LimitPerYear is Nullable 
        ///</summary>


        [DisplayName("Limit Per Year")]
        public int? LimitPerYear { get; set; }

        ///<summary>
        /// Get or Set the ExpiryDate Property of WorkOrderClientAgreementEntityProduct
        /// ExpiryDate is Nullable 
        ///</summary>


        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        ///<summary>
        /// Get or Set the DefaultOrder Property of WorkOrderClientAgreementEntityProduct
        /// DefaultOrder is Nullable 
        ///</summary>

        [DisplayName("Default Order")]
        public int? DefaultOrder { get; set; }

        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Year")]
        public int? TonnageBilling { get; set; }

        [DisplayName("IsOngoing Discount")]
        public bool IsOngoingDiscount { get; set; }

        #endregion
    }
}
