using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayPoint.Model.ViewModels
{
    public class UserPermissionViewModel
    {
        public bool CanView { get; set; }

        public bool CanCreate { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool CanApprove { get; set; }

        public string ApprovalButtonstatus { get; set; }

        public bool CanActivateDeletion { get; set; }

        public bool CanActivate { get; set; }

        public bool CanRereview { get; set; }
        public bool isAwaitingPayment { get; set; }
    }
}
