using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionSystem.Application.Services.Interfaces;
using PermissionSystem.Contracts.DTOs.Users;
using PermissionSystem.Domain.Entities;

namespace PermissionSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// The user service used for managing users.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService"> The user service.</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns> A list of all users. </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Gets a user by its ID.
        /// </summary>
        /// <param name="id"> The ID of the user. </param>
        /// <returns> The user with the specified ID. </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user"> The user entity. </param>
        /// <returns> The created user. </returns>
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(User user)
        {
            var created = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id"> The ID of the user to update. </param>
        /// <param name="user"> The user entity. </param>
        /// <returns> The updated user. </returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Update(int id, User user)
        {
            if (id != user.Id) return BadRequest();
            var updated = await _userService.UpdateAsync(user);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes a user by its ID.
        /// </summary>
        /// <param name="id"> The ID of the user to delete. </param>
        /// <returns> No content if deleted, NotFound if user does not exist. </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _userService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
