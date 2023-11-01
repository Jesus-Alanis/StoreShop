using Catalog.Application;
using Catalog.Application.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Catalog.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CategoriesController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Application.DTOs.Category>), StatusCodes.Status200OK)]
        public async Task<IResult> GetCategories()
        {
            var categories = await _catalogService.GetCategoriesAsync().ConfigureAwait(false);
            return Results.Ok(categories.Select(c => c.ToDto()).AsEnumerable());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Application.DTOs.Category), StatusCodes.Status200OK)]
        public async Task<IResult> GetCategory(long id)
        {
            var category = await _catalogService.GetCategoryAsync(id).ConfigureAwait(false);
            return Results.Ok(category.ToDto());
        }

        [HttpPost()]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Application.DTOs.Category), StatusCodes.Status201Created)]
        public async Task<IResult> CreateCategory(Application.DTOs.Category dto)
        {
            var categoryId = await _catalogService.AddCategoryAsync(dto).ConfigureAwait(false);
            var location = Url.Action(nameof(GetCategory), new { id = categoryId }) ?? $"/{categoryId}";
            return Results.Created(location, dto);
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> UpdateCategory(long id, [FromBody] Application.DTOs.Category dto)
        {
            await _catalogService.UpdateCategoryAsync(id, dto).ConfigureAwait(false);
            return Results.Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> DeleteCategory(long id)
        {
            await _catalogService.RemoveCategoryAsync(id).ConfigureAwait(false);
            return Results.Ok();
        }

    }
}
