using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CST.Dal.SqlContext
{
    internal class CstContextFactory : ICstContextFactory
    {
        private readonly string connectionString;

        public CstContextFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public CstContext CreateContext()
        {
            var opt = new DbContextOptionsBuilder<CstContext>().UseNpgsql(connectionString)
                .LogTo(s => Debug.WriteLine(s), new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                .EnableSensitiveDataLogging().Options;
            return new CstContext(opt);
        }
    }
}
