using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Netwealth.Common.Models.Data;

namespace Netwealth.Dal
{
    // https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli
    public class NetwealthDbContextFactory : IDesignTimeDbContextFactory<NetwealthDbContext>
    {
        public NetwealthDbContext CreateDbContext(string[] args)
        {
            var assembly = typeof(NetwealthDbContext).Assembly;
            var configurationBuilder =
                new ConfigurationBuilder()
                    .AddUserSecrets(assembly);

            var config = configurationBuilder.Build();
            var connectionString = config["Netwealth:ConnectionString"];

            var builder = new DbContextOptionsBuilder<NetwealthDbContext>();
            builder
                .UseSqlServer(connectionString)
                .EnableDetailedErrors();

            return new NetwealthDbContext(builder.Options);
        }
    }

    public class NetwealthDbContext : DbContext
    {
        private string connectionString = string.Empty;

        public NetwealthDbContext()
        {
            var assembly = typeof(NetwealthDbContext).Assembly;
            var configurationBuilder = 
                new ConfigurationBuilder()
                    .AddUserSecrets(assembly);
            
            var config = configurationBuilder.Build();
            connectionString = config["Netwealth:ConnectionString"];
        }

        public NetwealthDbContext(DbContextOptions<NetwealthDbContext> options) : base(options)  // ConnectionString is passed via the "options"
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options
                    .UseSqlServer(connectionString)
                    .EnableDetailedErrors();
            }
        }

        public DbSet<Currency> Currencies { get; set; }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder
            //    .Entity<Currency>()
            //    .ToTable("Currencies");
            modelBuilder
                .Entity<Currency>()
                .Property(p => p.Rate)
                    .HasColumnType("Decimal(20,14)");
            modelBuilder
                .Entity<Currency>()
                .Property(p => p.InverseRate)
                    .HasColumnType("Decimal(20,14)");
        }
        #endregion
    }
}