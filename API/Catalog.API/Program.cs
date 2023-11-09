using StoreShop.Infra;

var builder = WebApplication.CreateBuilder(args);

DependencyContainer.RegisterDatabase(builder.Services, builder.Configuration);
DependencyContainer.RegisterServices(builder.Services);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
