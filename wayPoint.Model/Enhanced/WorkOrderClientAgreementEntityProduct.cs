using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayPoint.Model
{
    public partial class WorkOrderClientAgreementEntityProduct
    {

        #region Properties

        /// <summary>
        /// Get or Set Discount Type Name Property
        /// </summary>
        public string DiscountTypeName { get; set; }

        /// <summary>
        /// Get or Set System Product Name Property
        /// </summary>
        public string SystemProductName { get; set; }


        /// <summary>
        /// Get or Set System Product Amount Property
        /// </summary>
        public decimal? SystemProductAmount { get; set; }

        ///<summary>
        /// Get or Set the SystemDiscountScheduleId Property 
        /// SystemDiscountScheduleId is Nullable 
        ///</summary>
        public int? SystemDiscountScheduleId { get; set; }

        ///<summary>
        /// Get or Set the SystemDiscountScheduleName Property 
        ///</summary>
        public string SystemDiscountScheduleName { get; set; }

        /////<summary>
        ///// Get or Set the DefaultOrder Property 
        ///// DefaultOrder is Nullable 
        /////</summary>
        //public int? DefaultOrder { get; set; }

        /// <summary>
        /// Get or Set System Discount Program Type Id Property
        /// </summary>
        public int? SystemDiscountProgramTypeId { get; set; }

        /// <summary>
        /// Get or Set Entity Id Property
        /// </summary>
        public int? EntityId { get; set; }

        public string ProductCode { get; set; }
        public int? ProductGroupTypeId { get; set; }
        public int? TonnageBillingReference { get; set; }


        //public int? WorkOrderId { get; set; }
        #endregion

    } // end of class
}
