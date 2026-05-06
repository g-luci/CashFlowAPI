using CashFlow.Application.UseCases.Users.Register;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Users.Register
{
    public class RegisterUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            //Arrange
            var validator = new RegisterUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeTrue();
        }
    }
}
