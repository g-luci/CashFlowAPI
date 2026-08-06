using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Users.Update
{
    public class UpdateUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new UpdateUserValidator();
            var request = RequestUpdateUserJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData(null)]
        public void Error_Name_Empty(string name)
        {
            var validator = new UpdateUserValidator();
            var request  = RequestUpdateUserJsonBuilder.Build();
            request.Name = name;

            var result = validator.Validate(request);   

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(one => one.Count().ShouldBe(1),
                two => two.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY)));

        }

        [Theory]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData(null)]
        public void Error_Email_Empty(string email)
        {
            var validator = new UpdateUserValidator();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = email;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(one => one.Count().ShouldBe(1),
                two => two.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY)));
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            var validator = new UpdateUserValidator();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "invalid.com";

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(one => one.Count().ShouldBe(1),
                two => two.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID)));
        }
    }
}
