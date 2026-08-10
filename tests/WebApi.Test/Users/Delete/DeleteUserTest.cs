using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net;

namespace WebApi.Test.Users.Delete
{
    public class DeleteUserTest : CashFlowClassFixture
    {
        private const string METHOD = "api/User";
        private const string METHOD_LOGIN = "api/Login";

        private readonly string _token;
        private readonly string _email;
        private readonly string _password;

        public DeleteUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _email = webApplicationFactory.User_Team_Member.GetEmail();
            _password = webApplicationFactory.User_Team_Member.GetPassword();
        }

        [Fact]
        public async Task Success()
        {
            var result = await DoDelete(METHOD, _token);

            result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            var request = new RequestLoginJson { Email = _email, Password = _password };

            var response = await DoPost(requestUri: METHOD_LOGIN, request: request);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
