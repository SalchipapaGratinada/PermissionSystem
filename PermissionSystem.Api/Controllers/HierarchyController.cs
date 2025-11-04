using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionSystem.Application.Services.Implementations;
using PermissionSystem.Application.Services.Interfaces;
using PermissionSystem.Contracts.DTOs.Hierarchies;
using PermissionSystem.Domain.Entities;

namespace PermissionSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HierarchyController : ControllerBase
    {
        /// <summary>
        /// The hierarchy service used for managing hierarchy nodes.
        /// </summary>
        private readonly IHierarchyService _hierarchyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyController"/> class.
        /// </summary>
        /// <param name="hierarchyService">The hierarchy service.</param>
        public HierarchyController(IHierarchyService hierarchyService)
        {
            _hierarchyService = hierarchyService;
        }

        /// <summary>
        /// Gets all hierarchy nodes.
        /// </summary>
        /// <returns>A list of all hierarchy nodes. </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HierarchyNodeDto>>> GetAll()
        {
            var nodes = await _hierarchyService.GetAllAsync();
            return Ok(nodes);
        }

        /// <summary>
        /// Gets a hierarchy node by its ID.
        /// </summary>
        /// <param name="id">The ID of the hierarchy node.</param>
        /// <returns>The hierarchy node with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<HierarchyNodeDto>> GetById(int id)
        {
            var node = await _hierarchyService.GetByIdAsync(id);
            if (node == null) return NotFound();
            return Ok(node);
        }

        /// <summary>
        /// Creates a new hierarchy node.
        /// </summary>
        /// <param name="node">The hierarchy node.</param>
        /// <returns>The created hierarchy node.</returns>
        [HttpPost]
        public async Task<ActionResult<HierarchyNodeDto>> Create(HierarchyNode node)
        {
            var created = await _hierarchyService.CreateAsync(node);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing hierarchy node.
        /// </summary>
        /// <param name="id">The ID of the hierarchy node.</param>
        /// <param name="node">The hierarchy node.</param>
        /// <returns>The updated hierarchy node.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<HierarchyNodeDto>> Update(int id, HierarchyNode node)
        {
            if (id != node.Id) node.Id = id; // fuerza el id desde la ruta
            var updated = await _hierarchyService.UpdateAsync(node);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes a hierarchy node by its ID.
        /// </summary>
        /// <param name="id">The ID of the hierarchy node.</param>
        /// <returns>No content if deleted, NotFound otherwise.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _hierarchyService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Gets the children of a hierarchy node by its parent ID.
        /// </summary>
        /// <param name="parentId">The ID of the parent hierarchy node.</param>
        /// <returns>A list of child hierarchy nodes.</returns>
        [HttpGet("{parentId}/children")]
        public async Task<ActionResult<IEnumerable<HierarchyNodeDto>>> GetChildren(int parentId)
        {
            var children = await _hierarchyService.GetChildrenAsync(parentId);
            return Ok(children);
        }
    }
}
