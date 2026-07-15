using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;

namespace UseCases.Test.Expenses.Register
{
    public class RegisterExpenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            var useCase = CreateUseCase(loggedUser);

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Title.ShouldBe(request.Title);
        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            var loggedUser = UserBuilder.Build();

            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Title = string.Empty;

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(request);

            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

            result.GetErros().ShouldSatisfyAllConditions(conditionOne => conditionOne.Count().ShouldBe(1),
                conditionTwo => conditionTwo.ShouldContain(ResourceErrorMessages.TITLE_REQUIRED));
        }

        private RegisterExpenseUseCase CreateUseCase(CashFlow.Domain.Entities.User user)
        {
            var repository = ExpensesWriteOnlyRepositoryBuilder.Build();
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new RegisterExpenseUseCase(repository, unitOfWork, mapper, loggedUser);
        }
    }
}
