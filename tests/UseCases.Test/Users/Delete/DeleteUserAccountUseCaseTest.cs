using CashFlow.Application.UseCases.Users.Delete;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using Shouldly;

namespace UseCases.Test.Users.Delete
{
    public class DeleteUserAccountUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            var act = async () => await useCase.Execute();

            await act.ShouldNotThrowAsync();
        }

        private DeleteUserAccountUseCase CreateUseCase(User user)
        {
            var unitOfWork = UnitOfWorkBuilder.Build();
            var repository = UserWriteOnlyRepositoryBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new DeleteUserAccountUseCase(unitOfWork, repository, loggedUser);
        }
    }
}
