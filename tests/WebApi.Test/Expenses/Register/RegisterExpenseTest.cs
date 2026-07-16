using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;


namespace WebApi.Test.Expenses.Register
{
    public class RegisterExpenseTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Expenses";
        private readonly string _token;

        public RegisterExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.GetToken();
        }

        [Fact]

        public async Task Success()
        {
            var request = RequestRegisterExpenseJsonBuilder.Build();

            var result = await DoPost(requestUri: METHOD, request: request, token: _token);

            result.StatusCode.ShouldBe(HttpStatusCode.Created);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            response.RootElement.GetProperty("title").GetString().ShouldBe(request.Title);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        
        public async Task Error_Title_Empty(string culture)
        {
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Title = string.Empty;

            var result = await DoPost(requestUri: METHOD, request: request, token: _token, culture: culture);

            result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(condition_one => condition_one.Count().ShouldBe(1),
                condition_two => condition_two.ShouldContain(error => error.GetString()!.Equals(expectedMessage)));
        }
    }
}
