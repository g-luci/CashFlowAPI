using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Register
{
    public class RegisterUserTest : CashFlowClassFixture
    {
        private const string METHOD = "api/User";
        public RegisterUserTest(CustomWebApplicationFactory webAppAplicationFactory) : base(webAppAplicationFactory)
        {
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var result = await DoPost(requestUri: METHOD, request: request);

            result.StatusCode.ShouldBe(HttpStatusCode.Created);

            var body =  await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            response.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
            response.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrEmpty();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var result = await DoPost(requestUri: METHOD, request: request, culture: culture);

            result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(condition_one => condition_one.Count().ShouldBe(1),
                condition_two => condition_two.ShouldContain(error => error.GetString()!.Equals(expectedMessage)));
        }
    }
}
