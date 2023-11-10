using Catalog.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

DependencyContainer.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
