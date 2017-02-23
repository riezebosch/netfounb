using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Reflection;
using System.Security;

namespace NETFOUNB.Tests
{
    [TestClass]
    public class RBACClaims
    {
        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void TestMethod1()
        {
            DezeMethodeMagAlleenDoorAdministatorsWordenUitgevoerd();
        }

        [PrincipalPermission(SecurityAction.Demand, Role = @"Scrum Master")]
        private void DezeMethodeMagAlleenDoorAdministatorsWordenUitgevoerd()
        {
        }

        [TestMethod]
        public void WieBenIk()
        {
            var identity = WindowsIdentity.GetCurrent();
            Assert.AreEqual(@"DOCENTD\Administrator", identity.Name);
        }

        [TestMethod]
        public void PrincipalPermissionMetEigenPrincipal()
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("Pietje"), new[] { @"Scrum Master" });
            DezeMethodeMagAlleenDoorAdministatorsWordenUitgevoerd();
        }

        [TestMethod]
        public void HeeftDePrincipalPermissionEffectOpReflection()
        {
            try
            {
                this.GetType().GetMethod(nameof(DezeMethodeMagAlleenDoorAdministatorsWordenUitgevoerd), BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this, null);
                Assert.Fail("als het goed is bereiken we dit punt nooit omdat hierboven een exception optrad.");
            }
            catch (TargetInvocationException ex) when (ex.InnerException is SecurityException)
            {
            }
        }

        [TestMethod]
        public void PrincipalPermissionImperatief()
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("Pietje"), new[] { @"Scrum Master" });

            if (true)
            {
                var perm = new PrincipalPermission(null, "Scrum Master");
                perm.Demand(); 
            }
        }
    }
}
