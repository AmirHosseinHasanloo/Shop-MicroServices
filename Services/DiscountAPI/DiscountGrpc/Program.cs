using Discount.Grpc.Repositories;
using Discount.Grpc.Extentions;
//using DiscountGrpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

#region IOC

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

#endregion

var app = builder.Build();

// Migration for postgresql =>
app.MigrateDataBase<Program>();

// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
