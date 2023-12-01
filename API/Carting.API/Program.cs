using Carting.API.Middlewares;
using Carting.API.Model.Swagger;
using Carting.Infra.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Identity.Web;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<CustomSwaggerOptions>();

builder.Services.ConfigureOptions<CustomSwaggerUIOptions>();

DependencyContainer.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(options =>
        {
            builder.Configuration.Bind("AzureAd", options);
        },
        options => { builder.Configuration.Bind("AzureAd", options); });

app.UseMiddleware<RequestLoggingMiddleware>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
