using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayPoint.Model
{
    public partial class ClientRole : BaseModel
    {
        #region Properties

        ///<summary>
        /// returns the schema name of the table
        ///</summary>
        public override string SchemaName
        {
            get
            {
                return "FN.usp_";
            }
        } // end of schema name property 

        ///<summary>
        /// Get or Set the Id property of ClientRole
        ///</summary>
        public int Id { get { return ClientRoleId; } set { ClientRoleId = value; } }

        ///<summary>
        /// Get or Set the ClientRoleId Property of ClientRole
        /// ClientRoleId is Not Nullable
        ///</summary>


        [DisplayName("Client Role Id")]
        public int ClientRoleId { get; set; }

        ///<summary>
        /// Get or Set the ClientId Property of ClientRole
        /// ClientId is Not Nullable
        ///</summary>


        [DisplayName("Client Id")]
        public int ClientId { get; set; }

        ///<summary>
        /// Get or Set the DepartmentRoleId Property of ClientRole
        /// DepartmentRoleId is Not Nullable
        ///</summary>


        [DisplayName("Department Role Id")]
        public int DepartmentRoleId { get; set; }

        ///<summary>
        /// Get or Set the EntityId Property of ClientRole
        /// EntityId is Nullable 
        ///</summary>


        [DisplayName("Entity Id")]
        public int? EntityId { get; set; }

        ///<summary>
        /// Get or Set the FromDate Property of ClientRole
        /// FromDate is Nullable 
        ///</summary>


        [DisplayName("From Date")]
        public DateTime? FromDate { get; set; }

        ///<summary>
        /// Get or Set the ToDate Property of ClientRole
        /// ToDate is Nullable 
        ///</summary>


        [DisplayName("To Date")]
        public DateTime? ToDate { get; set; }

        #endregion
    }
}
