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

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };

                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                                                ProductName VARCHAR(200) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
                    command.ExecuteNonQuery();

                    //seed data :

                    command.CommandText = @"INSERT INTO Coupon(ProductName,Description,Amount) 
                                            VALUES ('IPhone X','Is a good phone',1000)";

                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT INTO Coupon(ProductName,Description,Amount) 
                                            VALUES ('Samsung 10','Is a nice phone',1200)";

                    command.ExecuteNonQuery();

                    logger.LogInformation("Migration has been completed...");
                }

                catch (NpgsqlException exception)
                {
                    logger.LogError($"There is error :  {exception.Message}");

                    if (reteryForAvailibility < 50)
                    {
                        reteryForAvailibility++;
                        Thread.Sleep(2000);
                        MigrateDataBase<TContext>(host, reteryForAvailibility);
                    }

                }
            }
            return host;
        }
    }
}
