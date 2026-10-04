using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Business.Interfaces;
using ProductService.Models;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // ==========================================
        // GET ALL - Any logged-in user
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories =
                await _categoryService.GetAllCategoriesAsync();

            return Ok(categories);
        }

        // ==========================================
        // GET BY ID - Any logged-in user
        // ==========================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category =
                await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // ==========================================
        // POST - Admin only
        // ==========================================

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            try
            {
                var createdCategory =
                    await _categoryService.CreateCategoryAsync(category);

                return Ok(createdCategory);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // PUT - Admin only
        // ==========================================

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            try
            {
                await _categoryService.UpdateCategoryAsync(category);

                return NoContent();
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // DELETE - Admin only
        // ==========================================

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);

            return NoContent();
        }
    }
}