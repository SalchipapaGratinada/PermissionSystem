using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionSystem.Application.Services.Implementations;
using PermissionSystem.Application.Services.Interfaces;
using PermissionSystem.Contracts.DTOs.Permissions;
using PermissionSystem.Domain.Entities;

namespace PermissionSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        /// <summary>
        /// The permission service used for managing permissions.
        /// </summary>
        private readonly IPermissionService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsController"/> class.
        /// </summary>
        /// <param name="service"> The permission service.</param>
        public PermissionsController(IPermissionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all permissions.
        /// </summary>
        /// <returns> A list of all permissions. </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetAll()
        {
            var permissions = await _service.GetAllAsync();
            return Ok(permissions);
        }

        /// <summary>
        /// Gets a permission by its ID.
        /// </summary>
        /// <param name="id"> The ID of the permission. </param>
        /// <returns> The permission with the specified ID. </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionDto>> GetById(int id)
        {
            var permission = await _service.GetByIdAsync(id);
            if (permission == null) return NotFound();
            return Ok(permission);
        }

        /// <summary>
        /// Creates a new permission.
        /// </summary>
        /// <param name="dto"> The permission data transfer object. </param>
        /// <returns> The created permission. </returns>
        [HttpPost]
        public async Task<ActionResult<PermissionDto>> Create(PermissionDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing permission.
        /// </summary>
        /// <param name="id"> The ID of the permission to update. </param>
        /// <param name="dto"> The permission data transfer object. </param>
        /// <returns> No content if successful; otherwise, an error response. </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PermissionDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var success = await _service.UpdateAsync(dto);
            if (!success) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes a permission by its ID.
        /// </summary>
        /// <param name="id"> The ID of the permission to delete. </param>
        /// <returns> No content if successful; otherwise, NotFound. </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
