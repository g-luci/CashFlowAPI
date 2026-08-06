using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;

namespace UseCases.Test.Users.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]

        public async Task Success()
        {
            var user = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            var act = async () => await useCase.Execute(request);

            await act.ShouldNotThrowAsync();

            user.Name.ShouldBe(request.Name);
            user.Email.ShouldBe(request.Email);
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            var user = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase(user);

            var act = async () => await useCase.Execute(request);

            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

            result.ShouldSatisfyAllConditions(one => one.GetErros().Count.ShouldBe(1),
                two => two.GetErros().ShouldContain(ResourceErrorMessages.NAME_EMPTY));
        }

        [Fact]
        public async Task Error_Email_Already_Exist()
        {
            var user = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(user, request.Email);

            var act = async () => await useCase.Execute(request);

            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

            result.ShouldSatisfyAllConditions(one => one.GetErros().Count.ShouldBe(1),
                two => two.GetErros().ShouldContain(ResourceErrorMessages.EMAIL_ALREADY_REGISTRED));
        }

        private UpdateUserUseCase CreateUseCase(User user, string? email = null)
        {
            var unitOfWork = UnitOfWorkBuilder.Build();
            var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
            var loggedUser = LoggedUserBuilder.Build(user);
            var readRepository = new UserReadOnlyRepositoryBuilder();

            if(string.IsNullOrWhiteSpace(email) == false)
            {
                readRepository.ExistActiveUserWithEmail(email);
            }

            return new UpdateUserUseCase(loggedUser, updateRepository, readRepository.Build(), unitOfWork);
        }
    }
}
