using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {
        public static IPasswordEncripter Build()
        {
            var mock = new Mock<IPasswordEncripter>();

            mock.Setup(passwrodEncripterConfig => 
            passwrodEncripterConfig.Encrypt(It.IsAny<string>())).Returns("!$@zchxziuchzxi#duashdi^uash@uhv-cbuyxzhc$");

            return mock.Object;
        }
    }
}
