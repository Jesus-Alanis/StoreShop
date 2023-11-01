using Carting.API.Model.Swagger;
using Carting.Infra.IoC;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<CustomSwaggerOptions>();

builder.Services.ConfigureOptions<CustomSwaggerUIOptions>();

DependencyContainer.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddApiVersioning(setup =>
{
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    setup.ReportApiVersions = true;
    setup.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
