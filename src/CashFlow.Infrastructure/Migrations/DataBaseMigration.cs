using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure.Migrations
{
    public class DataBaseMigration
    {
        public async static Task MigrationDatabase(IServiceProvider serviceProvider)
        {
           var dbcontext = serviceProvider.GetRequiredService<CashFlowDbContext>();

            await dbcontext.Database.MigrateAsync();
        }
    }
}
