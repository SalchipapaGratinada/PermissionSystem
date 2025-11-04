using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionSystem.Application.Services.Implementations;
using PermissionSystem.Application.Services.Interfaces;
using PermissionSystem.Contracts.DTOs.Grants;
using PermissionSystem.Contracts.DTOs.Permissions;
using PermissionSystem.Domain.Entities;

namespace PermissionSystem.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GrantsController : ControllerBase
    {
        /// <summary>
        /// The grant service used for managing grants.
        /// </summary>
        private readonly IGrantService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrantsController"/> class.
        /// </summary>
        /// <param name="service"></param>
        public GrantsController(IGrantService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all grants.
        /// </summary>
        /// <returns>A list of all grants. </returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var grants = await _service.GetAllAsync();
            return Ok(grants);
        }

        /// <summary>
        /// Gets a grant by its ID.
        /// </summary>
        /// <param name="id">The ID of the grant.</param>
        /// <returns>The grant with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var grant = await _service.GetByIdAsync(id);
            if (grant == null) return NotFound();
            return Ok(grant);
        }

        /// <summary>
        /// Creates a new grant.
        /// </summary>
        /// <param name="dto">The grant data transfer object.</param>
        /// <returns>The created grant.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(GrantDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing grant.
        /// </summary>
        /// <param name="id">The ID of the grant to update.</param>
        /// <param name="dto">The updated grant data transfer object.</param>
        /// <returns>No content if successful; otherwise, NotFound.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, GrantDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var success = await _service.UpdateAsync(dto);
            if (!success) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes a grant by its ID.
        /// </summary>
        /// <param name="id">The ID of the grant to delete.</param>
        /// <returns>No content if successful; otherwise, NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Gets permissions granted to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of permissions granted to the user.</returns>
        [HttpGet("user/{userId}/permissions")]
        public async Task<IActionResult> GetPermissionsByUser(int userId)
        {
            var permissions = await _service.GetPermissionsByUserAsync(userId);
            return Ok(permissions);
        }

        /// <summary>
        /// Gets permissions granted to a specific hierarchy node.
        /// </summary>
        /// <param name="hierarchyNodeId">The ID of the hierarchy node.</param>
        /// <returns>A list of permissions granted to the hierarchy node.</returns>
        [HttpGet("hierarchy/{hierarchyNodeId}/permissions")]
        public async Task<IActionResult> GetPermissionsByHierarchy(int hierarchyNodeId)
        {
            var permissions = await _service.GetPermissionsByHierarchyAsync(hierarchyNodeId);
            return Ok(permissions);
        }
    }
}
