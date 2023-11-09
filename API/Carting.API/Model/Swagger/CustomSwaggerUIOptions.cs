using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Carting.API.Model.Swagger
{
    public class CustomSwaggerUIOptions : IConfigureNamedOptions<SwaggerUIOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public CustomSwaggerUIOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }


        public void Configure(string name, SwaggerUIOptions options)
        {
            Configure(options);
        }

        public void Configure(SwaggerUIOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
            }
        }
    }
}
