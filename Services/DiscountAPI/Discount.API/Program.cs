using Discount.API.Repositories;
using Discount.API.Extentions;


var builder = WebApplication.CreateBuilder(args);
//migration

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region IOC

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

#endregion


var app = builder.Build();


// Migration for postgresql =>
app.MigrateDataBase<Program>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
