using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin
{
    public class DoLoginTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Login";
        private readonly string _email;
        private readonly string _name;
        private readonly string _password;

        public DoLoginTest(CustomWebApplicationFactory webAppAplicationFactory) : base(webAppAplicationFactory)
        {
            _email = webAppAplicationFactory.GetEmail();
            _name = webAppAplicationFactory.GetName();
            _password = webAppAplicationFactory.GetPassword();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestLoginJson { Email = _email, Password = _password };

            var response = await DoPost(requestUri: METHOD, request: request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().ShouldBe(_name);
            responseData.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrWhiteSpace();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Login_Invalid(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();

            var response = await DoPost(requestUri: METHOD, request: request, culture: culture);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(condition_one => condition_one.Count().ShouldBe(1),
                condition_two => condition_two.ShouldContain(error => error.GetString()!.Equals(expectedMessage)));
        }
    }
}
