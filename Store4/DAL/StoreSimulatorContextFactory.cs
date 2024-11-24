using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Store4.DAL
{
    public class StoreSimulatorContextFactory : IDesignTimeDbContextFactory<StoreSimulatorContext>
    {
        public StoreSimulatorContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StoreSimulatorContext>();
            optionsBuilder.UseSqlServer("Server=DUFFIWERPC\\SQLEXPRESS;Database=StoreSimulator;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

            return new StoreSimulatorContext(optionsBuilder.Options);
        }
    }
}
