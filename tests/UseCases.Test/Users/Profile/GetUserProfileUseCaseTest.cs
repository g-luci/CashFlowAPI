using CashFlow.Application.UseCases.Users.Profile;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using Shouldly;

namespace UseCases.Test.Users.Profile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            var result = await useCase.Execute();

            result.ShouldNotBeNull();
            result.Name.ShouldBe(user.Name);
            result.Email.ShouldBe(user.Email);
        }

        private GetUserProfileUseCase CreateUseCase(User user)
        {
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetUserProfileUseCase(mapper, loggedUser);
        }
    }
}
