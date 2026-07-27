using CashFlow.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Delete
{
    public class DeleteExpenseTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Expenses";

        private readonly string _token;
        private readonly long _expenseId;

        public DeleteExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _expenseId = webApplicationFactory.Expense.GetExpenseId();
        }

        [Fact]
        public async Task Success()
        {
            var result = await DoDelete(requestUri: $"{METHOD}/{_expenseId}", token: _token);

            result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            result = await DoGet(requestUri: $"{METHOD}/{_expenseId}", token: _token);

            result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Expense_Not_Found(string culture)
        {
            var result = await DoDelete(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

            result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(condition_one => condition_one.Count().ShouldBe(1),
                condition_two => condition_two.ShouldContain(error => error.GetString()!.Equals(expectedMessage)));
        }
    }
}
