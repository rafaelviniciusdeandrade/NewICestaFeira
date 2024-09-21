using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;


namespace CestaFeria.Data.Context
{
    public class SQLServerContextFactory : IDesignTimeDbContextFactory<ApsContext>
    {
        public ApsContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=USU-BCK1EGQQNPU; Database=CESTAFEIRA; User Id=teste; Password=123456;TrustServerCertificate=True;";

            var optionsBuilder = new DbContextOptionsBuilder<ApsContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new ApsContext(optionsBuilder.Options);
        }
    }
}