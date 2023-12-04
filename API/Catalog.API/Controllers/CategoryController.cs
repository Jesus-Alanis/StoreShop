using Catalog.Application;
using Catalog.Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Net.Mime;

namespace Catalog.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/categories")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class CategoryController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CategoryController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [RequiredScope("manager.read, buyer.read")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Application.DTOs.Category>), StatusCodes.Status200OK)]
        public async Task<IResult> GetCategories()
        {
            var categories = await _catalogService.GetCategoriesAsync();
            return Results.Ok(categories.Select(c => c.ToDto()).AsEnumerable());
        }

        [RequiredScope("manager.read, buyer.read")]
        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(Application.DTOs.Category), StatusCodes.Status200OK)]
        public async Task<IResult> GetCategory(long categoryId)
        {
            var category = await _catalogService.GetCategoryAsync(categoryId);
            return Results.Ok(category.ToDto());
        }

        [RequiredScope("manager.create")]
        [HttpPost()]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Application.DTOs.Category), StatusCodes.Status201Created)]
        public async Task<IResult> CreateCategory(Application.DTOs.Category dto)
        {
            var categoryId = await _catalogService.AddCategoryAsync(dto);
            var location = Url.Action(nameof(GetCategory), new { categoryId }) ?? $"/{categoryId}";
            return Results.Created(location, dto);
        }

        [RequiredScope("manager.update")]
        [HttpPut("{categoryId}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> UpdateCategory(long categoryId, [FromBody] Application.DTOs.Category dto)
        {
            await _catalogService.UpdateCategoryAsync(categoryId, dto);
            return Results.Ok();
        }

        [RequiredScope("manager.delete")]
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> DeleteCategory(long categoryId)
        {
            await _catalogService.RemoveCategoryAndItemsAsync(categoryId);
            return Results.Ok();
        }

    }
}
