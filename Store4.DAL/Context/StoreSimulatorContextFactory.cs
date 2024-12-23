﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Store4.DAL.Context
{
    public class StoreSimulatorContextFactory : IDesignTimeDbContextFactory<StoreSimulatorContext>
    {
        public StoreSimulatorContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StoreSimulatorContext>();
            optionsBuilder.UseSqlite("Data Source=Data/Store4.db");

            return new StoreSimulatorContext(optionsBuilder.Options);
        }
    }
}
