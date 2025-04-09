using Npgsql;

namespace Discount.API.Extentions
{

    public static class HostExtentions
    {
        public static IHost MigrateDataBase<TContext>(this IHost host, int? retry = 0)
        {

            int reteryForAvailibility = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                // migrate
                try
                {
                    logger.LogInformation("migrating Postgresql database ...");

                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                }
                catch (Exception)
                {

                    throw;
                }
            }
            return host;
        }
    }
}
