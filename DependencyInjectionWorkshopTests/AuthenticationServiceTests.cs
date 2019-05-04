using DependencyInjectionWorkshop.Models;
using NUnit.Framework;

namespace DependencyInjectionWorkshopTests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        [Test]
        public void is_valid()
        {
            var authenticationService = new AuthenticationService();

            var account = "";
            var password = "";
            var otp = "";
            
            var actual = authenticationService.Verify(account, password, otp);
        }
    }
}