using Catalog.Application;
using Catalog.Application.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Catalog.API.Controllers
{

    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CategoryController(ICatalogService catalogService)
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

        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(Application.DTOs.Category), StatusCodes.Status200OK)]
        public async Task<IResult> GetCategory(long categoryId)
        {
            var category = await _catalogService.GetCategoryAsync(categoryId).ConfigureAwait(false);
            return Results.Ok(category.ToDto());
        }

        [HttpPost()]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Application.DTOs.Category), StatusCodes.Status201Created)]
        public async Task<IResult> CreateCategory(Application.DTOs.Category dto)
        {
            var categoryId = await _catalogService.AddCategoryAsync(dto).ConfigureAwait(false);
            var location = Url.Action(nameof(GetCategory), new { categoryId }) ?? $"/{categoryId}";
            return Results.Created(location, dto);
        }

        [HttpPut("{categoryId}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> UpdateCategory(long categoryId, [FromBody] Application.DTOs.Category dto)
        {
            await _catalogService.UpdateCategoryAsync(categoryId, dto).ConfigureAwait(false);
            return Results.Ok();
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> DeleteCategory(long categoryId)
        {
            await _catalogService.RemoveCategoryAndItemsAsync(categoryId).ConfigureAwait(false);
            return Results.Ok();
        }

    }
}
