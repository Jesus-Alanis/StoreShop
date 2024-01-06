using Carting.gRPC.Interceptors;
using Carting.gRPC.Middlewares;
using Carting.gRPC.Services;
using Carting.Infra.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

DependencyContainer.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddGrpc(options => options.Interceptors.Add<ServerLoggerInterceptor>());

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(options =>
        {
            builder.Configuration.Bind("AzureAd", options);
        },
        options => { builder.Configuration.Bind("AzureAd", options); });

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<CorrelationIdContext>();

app.MapGrpcService<CartingService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();