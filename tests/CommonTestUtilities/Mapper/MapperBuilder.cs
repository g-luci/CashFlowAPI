using AutoMapper;
using CashFlow.Application.AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace CommonTestUtilities.Mapper
{
    public class MapperBuilder
    {
        public static IMapper Build()
        {
            var mapper = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapping());
            },
            NullLoggerFactory.Instance);

            return mapper.CreateMapper();
        }
    }
}
