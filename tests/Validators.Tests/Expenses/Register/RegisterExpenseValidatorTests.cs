using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Expenses.Register
{
    public class RegisterExpenseValidatorTests
    {
        [Fact]
        public void Success()
        {
            //Arrange
            var validator = new RegisterExpenseValidator();

            var request = RequestRegisterExpenseJsonBuilder.Build();

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("           ")]
        [InlineData(null)]
        public void Error_Title_Empty(string title)
        {
            //Arrange
            var validator = new RegisterExpenseValidator();

            var request = RequestRegisterExpenseJsonBuilder.Build();

            request.Title = title;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors
                .ShouldSatisfyAllConditions(condition_one => condition_one.ShouldHaveSingleItem(),
                condition_two => condition_two.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED)));
        }

        [Fact]
        public void Error_Date_Future()
        {
            //Arrange
            var validator = new RegisterExpenseValidator();

            var request = RequestRegisterExpenseJsonBuilder.Build();

            request.Date = DateTime.UtcNow.AddDays(1);

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors
                .ShouldSatisfyAllConditions(condition_one => condition_one.ShouldHaveSingleItem(),
                condition_two => condition_two.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EXPENSE_CANNOT_FOR_THE_FUTURE)));
        }

        [Fact]
        public void Error_Payment_Type_Invalid()
        {
            //Arrange
            var validator = new RegisterExpenseValidator();

            var request = RequestRegisterExpenseJsonBuilder.Build();

            request.PaymentType = (PaymentType)700;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors
                .ShouldSatisfyAllConditions(condition_one => condition_one.ShouldHaveSingleItem(),
                condition_two => condition_two.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID)));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Error_Amount_Invalid(decimal amount)
        {
            //Arrange
            var validator = new RegisterExpenseValidator();

            var request = RequestRegisterExpenseJsonBuilder.Build();

            request.Amount = amount;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors
                .ShouldSatisfyAllConditions(condition_one => condition_one.ShouldHaveSingleItem(),
                condition_two => condition_two.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO)));
        }
    }
}
