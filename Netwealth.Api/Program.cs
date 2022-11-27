using Microsoft.EntityFrameworkCore;
using Netwealth.Dal;
using Netwealth.DataSeeder;

namespace Netwealth.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var assembly = typeof(NetwealthDbContext).Assembly;
            var configurationBuilder =
                new ConfigurationBuilder()
                    .AddUserSecrets(assembly);
            var config = configurationBuilder.Build();
            var connectionString = config["Netwealth:ConnectionString"];

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<NetwealthDbContext>(
                options =>
                    options.UseSqlServer(connectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Seed data...
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        new DataSeederProvider(services).PerformDataSeeding();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred seeding the database.");
                    }
                }

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}