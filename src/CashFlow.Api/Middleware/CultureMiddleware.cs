using System.Globalization;

namespace CashFlow.Api.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;
        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

            var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(requestedCulture))
            {
                requestedCulture = requestedCulture.Split(',')[0].Trim();
            }

            var cultureInfo = new CultureInfo("en-US");

            if (string.IsNullOrWhiteSpace(requestedCulture) == false 
                && supportedLanguages.Exists(language => language.Name.Equals(requestedCulture))) 
            {
                cultureInfo = new CultureInfo(requestedCulture);
            }

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            await _next(context);
        }
    }
}
