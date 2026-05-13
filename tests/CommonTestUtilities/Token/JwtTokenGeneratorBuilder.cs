using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Moq;

namespace CommonTestUtilities.Token
{
    public class JwtTokenGeneratorBuilder
    {
        public static IAccessTokenGenerator Build()
        {
            var mock = new Mock<IAccessTokenGenerator>();
                
            mock.Setup(accessTokenGeneratorConfig => 
                accessTokenGeneratorConfig
                .Generate(It.IsAny<User>()))
                .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IlRlc3QiLCJhZG1pbiI6dHJ1ZSwiaWF0IjoxNTE2MjM5MDIyfQ.zVf_WAdU9oOU6ORHGws8Hh7RsobvSzQgo3yOrM7Iht8");

            return mock.Object;
        }
    }
}
