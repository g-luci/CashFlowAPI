using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using Shouldly;

namespace UseCases.Test.Expenses.GetById
{
    public class GetExpenseByIdUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);

            var useCase = CreateUseCase(loggedUser, expense);

            var result = await useCase.Execute(expense.Id);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(expense.Id);
            result.Title.ShouldBe(expense.Title);
            result.Description.ShouldBe(expense.Description);
            result.Date.ShouldBe(expense.Date);
            result.Amount.ShouldBe(expense.Amount);
            result.PaymentType.ShouldBe((CashFlow.Communication.Enums.PaymentType)expense.PaymentType);
            result.Tags.ShouldSatisfyAllConditions(
                one => one.ShouldNotBeNull(),
                two => two.ShouldNotBeEmpty(), 
                three => three.ShouldBeEquivalentTo(expense.Tags.Select(tag => (CashFlow.Communication.Enums.Tag)tag.Value).ToList()));
        }

        [Fact]
        public async Task Error_Expense_Not_Found()
        {
            var loggedUser = UserBuilder.Build();

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(id: 1000);

            var result = await act.ShouldThrowAsync<NotFoundException>();

            result.ShouldSatisfyAllConditions(one => one.GetErros().Count.ShouldBe(1),
                two => two.GetErros().ShouldContain(ResourceErrorMessages.EXPENSE_NOT_FOUND));
        }

        private GetExpenseByIdUseCase CreateUseCase(User user, Expense? expense = null)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetExpenseByIdUseCase(repository, mapper, loggedUser);
        }
    }
}
