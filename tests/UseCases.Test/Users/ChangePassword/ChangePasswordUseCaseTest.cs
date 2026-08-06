using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;

namespace UseCases.Test.Users.ChangePassword
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUseCase(user, request.Password);

            var act = async () => await useCase.Execute(request);

            await act.ShouldNotThrowAsync();
        }

        [Fact]
        public async Task Error_NewPassword_Empty()
        {
            var user = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.NewPassword = string.Empty;

            var useCase = CreateUseCase(user, request.Password);

            var act = async () => { await useCase.Execute(request); };

            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

            result.ShouldSatisfyAllConditions(one => one.GetErros().Count.ShouldBe(1),
                two => two.GetErros().ShouldContain(ResourceErrorMessages.INVALID_PASSWORD));
        }

        [Fact]
        public async Task Error_CurrentPassword_Different()
        {
            var user = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            var act = async () => { await useCase.Execute(request); };

            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

            result.ShouldSatisfyAllConditions(one => one.GetErros().Count.ShouldBe(1),
                two => two.GetErros().ShouldContain(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        }

        public ChangePasswordUseCase CreateUseCase(User user, string? password = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var passwordEncripter = new PasswordEncrypterBuilder().Verify(password).Build();
            var repository = UserUpdateOnlyRepositoryBuilder.Build(user);
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new ChangePasswordUseCase(loggedUser, passwordEncripter, repository, unitOfWork);
        }
    }
}
