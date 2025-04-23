using Discount.Grpc.Repositories;
using Discount.Grpc.Extentions;
using Discount.Grpc.Services;
//using DiscountGrpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#region IOC

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

#endregion

var app = builder.Build();

// Migration for postgresql =>
app.MigrateDataBase<Program>();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
