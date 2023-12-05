using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Carting.API.Model.Swagger
{
    public class CustomSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public CustomSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            // add swagger document for every API version discovered
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                CreateVersionInfo(options, description);
            }

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            options.IncludeXmlComments(xmlFilePath);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\nEnter your token in the text input below. \r\n\r\nExample: \"12345abcdef\"",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                                    {
                                        {
                                             new OpenApiSecurityScheme
                                             {
                                                 Reference = new OpenApiReference
                                                 {
                                                     Type = ReferenceType.SecurityScheme,
                                                     Id = "Bearer"
                                                 }
                                             },
                                             Array.Empty<string>()
                                        }
                                    });

            options.CustomSchemaIds(x => x.FullName);
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private void CreateVersionInfo(SwaggerGenOptions options, ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = $"Carting API",
                Version = description.ApiVersion.ToString(),
                Description = "Carting API."
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            options.SwaggerDoc(description.GroupName, info);
        }
    }
}
