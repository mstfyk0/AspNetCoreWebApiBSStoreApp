using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Threading.Tasks;


namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly IServiceManager _services;

        public CategoryController(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            return Ok(await _services.CategoryService.GetAllCategoriesAsync(false));
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneCategoryByIdAsync([FromRoute] int id)
        {
            return Ok(await _services.CategoryService.GetOneCategoryByIdAsync(id,false));
        }

        [HttpPost(Name = "CreateOneCategoryAsync")]
        public async Task<IActionResult> CreateOneCategoryAsync([FromBody] CategoryDtoForInsertion category)
        {
            await _services.CategoryService.CreateOneCategory(category);
            return StatusCode(201, category);

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneCategoryAsync([FromRoute(Name = "id")] int id, [FromBody] CategoryDtoForUpdate categoryDto)
        {
            await _services.CategoryService.UpdateOneCategory(id, categoryDto, false);
            return Ok(categoryDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneCategporyAsync([FromRoute(Name = "id")] int id)
        {
            await _services.CategoryService.DeleteOneCategory(id, false);

            return NoContent();
        }
    }
}
