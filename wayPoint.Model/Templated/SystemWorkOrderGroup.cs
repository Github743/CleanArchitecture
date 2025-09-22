using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayPoint.Model.Templated
{
    public partial class SystemWorkOrderGroup : BaseModel
    {

        #region Properties

        ///<summary>
        /// returns the schema name of the table
        ///</summary>
        public override string SchemaName
        {
            get
            {
                return "meta.usp_";
            }
        } // end of schema name property 

        ///<summary>
        /// Get or Set the Id property of SystemWorkOrderGroup
        ///</summary>
        public int Id { get { return SystemWorkOrderGroupId; } set { SystemWorkOrderGroupId = value; } }

        ///<summary>
        /// Get or Set the SystemWorkOrderGroupId Property of SystemWorkOrderGroup
        /// SystemWorkOrderGroupId is Not Nullable
        ///</summary>


        [DisplayName("System Work Order Group Id")]
        public int SystemWorkOrderGroupId { get; set; }

        ///<summary>
        /// Get or Set the SystemGroupId Property of SystemWorkOrderGroup
        /// SystemGroupId is Not Nullable
        ///</summary>


        [DisplayName("System Group Id")]
        public int SystemGroupId { get; set; }

        ///<summary>
        /// Get or Set the SystemWorkOrderId Property of SystemWorkOrderGroup
        /// SystemWorkOrderId is Not Nullable
        ///</summary>


        [DisplayName("System Work Order Id")]
        public int SystemWorkOrderId { get; set; }

        ///<summary>
        /// Get or Set the CanView Property of SystemWorkOrderGroup
        /// CanView is Not Nullable
        ///</summary>


        [DisplayName("Can View")]
        public bool CanView { get; set; }

        ///<summary>
        /// Get or Set the CanEdit Property of SystemWorkOrderGroup
        /// CanEdit is Not Nullable
        ///</summary>


        [DisplayName("Can Edit")]
        public bool CanEdit { get; set; }

        ///<summary>
        /// Get or Set the CanCreate Property of SystemWorkOrderGroup
        /// CanCreate is Not Nullable
        ///</summary>


        [DisplayName("Can Create")]
        public bool CanCreate { get; set; }

        ///<summary>
        /// Get or Set the CanDelete Property of SystemWorkOrderGroup
        /// CanDelete is Not Nullable
        ///</summary>


        [DisplayName("Can Delete")]
        public bool CanDelete { get; set; }

        ///<summary>
        /// Get or Set the CanActivate Property of SystemWorkOrderGroup
        /// CanActivate is Not Nullable
        ///</summary>


        [DisplayName("Can Activate")]
        public bool CanActivate { get; set; }

        ///<summary>
        /// Get or Set the CanRereview Property of SystemWorkOrderGroup
        /// CanRereview is Not Nullable
        ///</summary>


        [DisplayName("Can Rereview")]
        public bool CanRereview { get; set; }

        #endregion
    }

}
