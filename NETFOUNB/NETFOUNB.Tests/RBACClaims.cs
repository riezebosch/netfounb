using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Permissions;
using System.Security.Principal;
using System.IdentityModel.Services;
using System.Security.Claims;

namespace NETFOUNB.Tests
{
    [TestClass]
    public class RBAC
    {
        [ClassInitialize]
        [TestMethod]
        [ClaimsPrincipalPermission(SecurityAction.Demand, Operation = "Print", Resource = "UPO")]
        public void Claims()
        {

        }
    }

    class ClaimsAuthorizationManager : System.Security.Claims.ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            return true;
            //return base.CheckAccess(context);
        }
    }
}
