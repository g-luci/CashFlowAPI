using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Users.Profile
{
    public class GetUserProfileUseCase : IGetUserProfileUseCase
    {
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetUserProfileUseCase(
            IMapper mapper,
            ILoggedUser loggedUser)
        {
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseUserProfileJson> Execute()
        {
            var user = await _loggedUser.Get();

            return _mapper.Map<ResponseUserProfileJson>(user);
        }
    }
}
